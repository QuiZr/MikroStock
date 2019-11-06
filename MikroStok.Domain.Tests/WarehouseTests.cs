using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using FluentValidation;
using Marten;
using Marten.Storage;
using MikroStok.CQRS.Core.Commands.Interfaces;
using MikroStok.CQRS.Core.Queries.Interfaces;
using MikroStok.Domain.Commands;
using MikroStok.Domain.Models;
using MikroStok.Domain.Queries;
using Npgsql;
using NUnit.Framework;

namespace MikroStok.Domain.Tests
{
    public class WarehouseTests
    {
        private IQueryBus _queryBus;
        private ICommandsBus _commandsBus;
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
            var query = new GetWarehousesQuery();
            var projectionResult = (await _queryBus.Query<GetWarehousesQuery, IReadOnlyList<Warehouse>>(query)).Single();

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
            var query = new GetWarehousesQuery();
            var projectionResult = await _queryBus.Query<GetWarehousesQuery, IReadOnlyList<Warehouse>>(query);

            Assert.IsEmpty(projectionResult);
        }

        [Test]
        public async Task WhenTwoCreated_CanBeReadUsingQuery()
        {
            // Arrange
            var createCommand1 = new CreateWarehouseCommand
            {
                Id = Guid.NewGuid(),
                Name = "łerhałs"
            };
            var createCommand2 = new CreateWarehouseCommand
            {
                Id = Guid.NewGuid(),
                Name = "łerhałsTWO"
            };

            // Act
            await _commandsBus.Send(createCommand1);
            await _commandsBus.Send(createCommand2);

            // Assert
            var query = new GetWarehousesQuery();
            var projectionResult = await _queryBus.Query<GetWarehousesQuery, IReadOnlyList<Warehouse>>(query);

            Assert.AreEqual(2, projectionResult.Count);
            Assert.True(projectionResult.Any(x => x.Name == createCommand1.Name));
            Assert.True(projectionResult.Any(x => x.Name == createCommand2.Name));
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