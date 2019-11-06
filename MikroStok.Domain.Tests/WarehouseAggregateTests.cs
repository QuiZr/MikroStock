using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using FluentValidation;
using Marten;
using Marten.Storage;
using MikroStok.CQRS.Core.Commands.Interfaces;
using MikroStok.Domain.Commands;
using MikroStok.Domain.Models;
using Npgsql;
using NUnit.Framework;

namespace MikroStok.Domain.Tests
{
    public class WarehouseAggregateTests
    {
        private ICommandsBus _commandsBus;
        private IContainer _container;
        private IDocumentStore _documentStore;

        [SetUp]
        public void Setup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new DomainTestsModule());
            _container = builder.Build();

            _commandsBus = _container.Resolve<ICommandsBus>();
            _documentStore = _container.Resolve<IDocumentStore>();
        }


        [Test]
        public async Task WhenCreated_CanBeReadProperly()
        {
            // Arrange
            var createCommand = new CreateWarehouseCommand
            {
                Id = Guid.NewGuid(),
                Name = "łerhałs"
            };

            // Act
            await _commandsBus.Send(createCommand);

            // Assert
            var projectionResult = await _documentStore.QuerySession()
                .Query<Warehouse>()
                .Where(x => x.Id == createCommand.Id)
                .SingleAsync();

            Assert.AreEqual(createCommand.Id, projectionResult.Id);
            Assert.AreEqual(createCommand.Name, projectionResult.Name);
            Assert.AreEqual(1, projectionResult.Version);
        }

        [Test]
        public void WhenCreatedWithInvalidName_ThrowsValidationException()
        {
            // Arrange
            var createCommand = new CreateWarehouseCommand
            {
                Id = Guid.NewGuid(),
                Name = string.Empty
            };

            // Act & Assert
            var validationException = Assert.CatchAsync<ValidationException>(() => _commandsBus.Send(createCommand));
            Assert.AreEqual(1, validationException.Errors.Count());
        }

        [Test]
        public async Task WhenCreatedAndDeleted_CanBeReadProperly()
        {
            // Arrange
            var createCommand = new CreateWarehouseCommand
            {
                Id = Guid.NewGuid(),
                Name = "łerhałs"
            };
            var deleteCommand = new DeleteWarehouseCommand(createCommand.Id, 2);

            // Act
            await _commandsBus.Send(createCommand);
            await _commandsBus.Send(deleteCommand);

            // Assert
            var projectionResult = await _documentStore.QuerySession()
                .Query<Warehouse>()
                .Where(x => x.Id == createCommand.Id)
                .SingleOrDefaultAsync();

            Assert.IsNull(projectionResult);
        }

        [TearDown]
        public async Task Cleanup()
        {
            var session = _documentStore.OpenSession();
            session.DeleteWhere<Warehouse>(x => true);
            await session.SaveChangesAsync();

            var connection = new NpgsqlConnection("host=localhost;database=mt5;username=postgres;password=postgres");
            using (connection.OpenAsync())
            {
                _documentStore.Events.RemoveAllObjects(new DdlRules(), connection);
            }
        }
    }
}