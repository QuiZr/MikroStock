using System.Threading.Tasks;
using Autofac;
using Baseline.Dates;
using Marten;
using Marten.Events.Projections.Async;
using Marten.Storage;
using MikroStok.CQRS.Core.Commands.Interfaces;
using MikroStok.CQRS.Core.Queries.Interfaces;
using MikroStok.Domain.Models;
using Npgsql;
using NUnit.Framework;

namespace MikroStok.Domain.Tests
{
    public class DomainTestBase
    {
        protected IQueryBus _queryBus;
        protected IDaemon _projectionDaemon;
        protected  ICommandsBus _commandsBus;
        private IDocumentStore _documentStore;

        [SetUp]
        public void Setup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new DomainTestsModule());
            var container = builder.Build();

            _queryBus = container.Resolve<IQueryBus>();
            _commandsBus = container.Resolve<ICommandsBus>();
            _documentStore = container.Resolve<IDocumentStore>();
            _projectionDaemon = _documentStore.BuildProjectionDaemon(settings: new DaemonSettings
            {
                LeadingEdgeBuffer = 50.Milliseconds(),
                FetchingCooldown = 50.Milliseconds()
            });
            _projectionDaemon.StartAll();
        }

        [TearDown]
        public async Task Cleanup()
        {
            await _projectionDaemon.StopAll();
            _projectionDaemon.Dispose();
            
            using (var session = _documentStore.OpenSession())
            {
                session.DeleteWhere<Warehouse>(x => true);
                session.DeleteWhere<Stock>(x => true);
                await session.SaveChangesAsync();
            }

            var connection = new NpgsqlConnection("host=localhost;database=mt5;username=postgres;password=postgres");
            using (connection.OpenAsync())
            {
                _documentStore.Events.RemoveAllObjects(new DdlRules(), connection);
            }
        }
    }
}