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
    public class WithdrawStockFromWarehouseCommandTests : DomainTestBase
    {
        [Test]
        public void WhenStockIsMissing_ThrowsValidationError()
        {
            // Arrange
            var withdrawCommand = new WithdrawStockFromWarehouseCommand()
            {
                Id = Guid.Empty,
                Version = 1,
                Count = 5,
            };
            
            // Act & Assert
            Assert.CatchAsync<ValidationException>(() => _commandsBus.Send(withdrawCommand));
        }
        
        [Test]
        public async Task WhenStockIsPresentAndCountInvalid_ThrowsValidationError()
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
            var withdrawCommand = new WithdrawStockFromWarehouseCommand()
            {
                Id = addStockCommand.Id,
                Version = 1,
                Count = -1,
            };
            
            await _commandsBus.Send(createWarehouseCommand);
            await _projectionDaemon.WaitForNonStaleResults();
            await _commandsBus.Send(addStockCommand);

            // Act & Assert
            var validationException = Assert.CatchAsync<ValidationException>(() => _commandsBus.Send(withdrawCommand));
            Assert.AreEqual(1, validationException.Errors.Count());
        }
        
        [Test]
        public async Task WhenWarehouseIsPresentAndStockValid_CanBeReadProperly()
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
            var withdrawCommand = new WithdrawStockFromWarehouseCommand()
            {
                Id = addStockCommand.Id,
                Version = 1,
                Count = 2,
            };
            
            // Act
            await _commandsBus.Send(createWarehouseCommand);
            await _projectionDaemon.WaitForNonStaleResults();
            await _commandsBus.Send(addStockCommand);
            await _commandsBus.Send(withdrawCommand);
            await _projectionDaemon.WaitForNonStaleResults();

            // Assert
            var query = new GetStockInWarehouseQuery
            {
                WarehouseId = createWarehouseCommand.Id
            };
            var queryResult = (await _queryBus.Query<GetStockInWarehouseQuery, IReadOnlyList<Stock>>(query)).Single();

            Assert.AreEqual(addStockCommand.Id, queryResult.Id);
            Assert.AreEqual(addStockCommand.ProductName, queryResult.ProductName);
            Assert.AreEqual(addStockCommand.Count - withdrawCommand.Count, queryResult.Count);
            Assert.AreEqual(2, queryResult.Version);
        }
    }
}