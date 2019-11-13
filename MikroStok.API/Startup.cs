using Autofac;
using Marten;
using Marten.Services.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MikroStok.CQRS.Core.Commands;
using MikroStok.CQRS.Core.Queries;
using MikroStok.Domain;
using MikroStok.Domain.Projections;
using MikroStok.ES.Core;
using MikroStok.ES.Core.Events;

namespace MikroStok.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(x => x.Filters.Add(typeof(ExceptionFilter)));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "MikroSztok", Version = "v1"});
            });
            
            services.AddOptions();
            services.AddHostedService<ProjectionDaemonHostedService>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new CommandsModule());
            builder.RegisterModule(new QueriesModule());
            builder.RegisterModule(new DomainModule());
            builder.RegisterModule(new EventsModule());
            builder.Register(_ => DocumentStore.For(x =>
            {
                x.Events.UseAggregatorLookup(AggregationLookupStrategy.UsePrivateApply);
                x.Connection(Configuration["ConnectionString"]);
                x.Events.AsyncProjections.Add<WarehouseProjection>();
                x.Events.AsyncProjections.Add<StockProjection>();
            })).As<IDocumentStore>();
            builder.Register(_ => new AggregateRepository(_.Resolve<IDocumentStore>())).As<IAggregateRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MikroSztok");
                c.RoutePrefix = string.Empty;
            });
            
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}