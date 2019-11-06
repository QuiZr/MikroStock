using Autofac;
using Marten;
using Marten.Services.Events;
using MikroStok.CQRS.Core.Commands;
using MikroStok.CQRS.Core.Queries;
using MikroStok.ES.Core;

namespace MikroStok.Domain.Tests
{
    public class DomainTestsModule  : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new CommandsModule());
            builder.RegisterModule(new QueriesModule());
            builder.RegisterModule(new DomainModule());
            builder.Register(_ => DocumentStore.For(x =>
            {
                x.Events.UseAggregatorLookup(AggregationLookupStrategy.UsePrivateApply);
                x.Connection("host=localhost;database=mt5;username=postgres;password=postgres");
            })).As<IDocumentStore>();
            builder.Register(_ => new AggregateRepository(_.Resolve<IDocumentStore>())).As<IAggregateRepository>();
        }
    }
}