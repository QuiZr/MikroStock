using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MikroStok.Domain.Commands;
using MikroStok.Domain.Models;
using MikroStok.Domain.Queries;
using NUnit.Framework;

namespace MikroStok.Domain.Tests
{
    public class AddStockToWarehouseCommandTests : DomainTestBase
    {
        [Test]
        public void WhenWarehouseIsMissing_ThrowsValidationError()
        {
            // Arrange
            var addStockCommand = new AddStockToWarehouseCommand()
            {
                Id = Guid.NewGuid(),
                ProductName = "łerhałs",
                Count = 5,
                WarehouseId = Guid.Empty
            };
            
            // Act & Assert
            var validationException = Assert.CatchAsync<ValidationException>(() => _commandsBus.Send(addStockCommand));
            Assert.AreEqual(1, validationException.Errors.Count());
        }
        
        [Test]
        public async Task WhenWarehouseIsPresentAndCountInvalid_ThrowsValidationError()
        {
            // Arrange
            var createWarehouseCommand = new CreateWarehouseCommand()
            {
                Id = Guid.NewGuid(),
                Name = "łerjhous"
            };
            var addStockCommand = new AddStockToWarehouseCommand()
            {
                Id = Guid.NewGuid(),
                ProductName = "łerhałs",
                Count = -1,
                WarehouseId = createWarehouseCommand.Id
            };
            
            await _commandsBus.Send(createWarehouseCommand);
            await _projectionDaemon.WaitForNonStaleResults();
            
            // Act & Assert
            var validationException = Assert.CatchAsync<ValidationException>(() => _commandsBus.Send(addStockCommand));
            Assert.AreEqual(1, validationException.Errors.Count());
        }
        
        [Test]
        public async Task WhenWarehouseIsPresent_CanBeReadProperly()
        {
            // Arrange
            var createWarehouseCommand = new CreateWarehouseCommand()
            {
                Id = Guid.NewGuid(),
                Name = "łerjhous"
            };
            var addStockCommand = new AddStockToWarehouseCommand()
            {
                Id = Guid.NewGuid(),
                ProductName = "łerhałs",
                Count = 5,
                WarehouseId = createWarehouseCommand.Id
            };
            
            // Act
            await _commandsBus.Send(createWarehouseCommand);
            await _projectionDaemon.WaitForNonStaleResults();
            await _commandsBus.Send(addStockCommand);
            await _projectionDaemon.WaitForNonStaleResults();

            // Assert
            var query = new GetStockInWarehouseQuery
            {
                WarehouseId = createWarehouseCommand.Id
            };
            var queryResult = (await _queryBus.Query<GetStockInWarehouseQuery, IReadOnlyList<Stock>>(query)).Single();

            Assert.AreEqual(addStockCommand.Id, queryResult.Id);
            Assert.AreEqual(addStockCommand.ProductName, queryResult.ProductName);
            Assert.AreEqual(addStockCommand.Count, queryResult.Count);
            Assert.AreEqual(1, queryResult.Version);
        }
    }
}