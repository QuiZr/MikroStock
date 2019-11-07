using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Baseline.Dates;
using FluentValidation;
using Marten;
using Marten.Events.Projections.Async;
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
    public class WarehouseTests : DomainTestBase
    {
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
            await _projectionDaemon.WaitForNonStaleResults();

            // Assert
            var query = new GetWarehousesQuery();
            var queryResult = (await _queryBus.Query<GetWarehousesQuery, IReadOnlyList<Warehouse>>(query)).Single();

            Assert.AreEqual(createCommand.Id, queryResult.Id);
            Assert.AreEqual(createCommand.Name, queryResult.Name);
            Assert.AreEqual(1, queryResult.Version);
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
            await _projectionDaemon.WaitForNonStaleResults();

            // Assert
            var query = new GetWarehousesQuery();
            var queryResult = await _queryBus.Query<GetWarehousesQuery, IReadOnlyList<Warehouse>>(query);

            Assert.IsEmpty(queryResult);
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
            await _projectionDaemon.WaitForNonStaleResults();

            // Assert
            var query = new GetWarehousesQuery();
            var queryResult = await _queryBus.Query<GetWarehousesQuery, IReadOnlyList<Warehouse>>(query);

            Assert.AreEqual(2, queryResult.Count);
            Assert.True(queryResult.Any(x => x.Name == createCommand1.Name));
            Assert.True(queryResult.Any(x => x.Name == createCommand2.Name));
            Assert.True(queryResult.Any(x => x.Id == createCommand1.Id));
            Assert.True(queryResult.Any(x => x.Id == createCommand2.Id));
            Assert.True(queryResult.All(x => x.Version == 1));
        }
    }
}