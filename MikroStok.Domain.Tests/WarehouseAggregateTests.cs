using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Baseline;
using FluentValidation;
using Marten;
using Marten.Services.Events;
using MikroStok.CQRS.Core.Commands;
using MikroStok.CQRS.Core.Commands.Interfaces;
using MikroStok.Domain.Aggregates;
using MikroStok.Domain.Commands;
using MikroStok.ES.Core;
using NUnit.Framework;

namespace MikroStok.Domain.Tests
{
    public class WarehouseAggregateTests
    {
        private IAggregateRepository _aggregateRepository;
        private ICommandsBus _commandsBus;
        private IContainer _container;
        [SetUp]
        public void Setup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new DomainTestsModule());
            _container = builder.Build();
            
            _commandsBus = _container.Resolve<ICommandsBus>();
            _aggregateRepository =  _container.Resolve<IAggregateRepository>();
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
            var aggregate = await _aggregateRepository.Load<WarehouseAggregate>(createCommand.Id);

            Assert.AreEqual(createCommand.Name, aggregate.Name);
            Assert.AreEqual(1, aggregate.Version);
            Assert.AreEqual(false, aggregate.IsDeleted);
        }

        [Test]
        public async Task WhenCreatedWithInvalidName_ThrowsValidationException()
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
            var deleteCommand = new DeleteWarehouseCommand(createCommand.Id);

            // Act
            await _commandsBus.Send(createCommand);
            await _commandsBus.Send(deleteCommand);

            // Assert
            var aggregate = await _aggregateRepository.Load<WarehouseAggregate>(createCommand.Id);

            Assert.AreEqual(createCommand.Name, aggregate.Name);
            Assert.AreEqual(2, aggregate.Version);
            Assert.AreEqual(true, aggregate.IsDeleted);
        }

        [Test]
        public async Task WhenCreatedAndDeleted_CanReadStateFromAfterCreatedEvent()
        {
            // Arrange
            var createCommand = new CreateWarehouseCommand
            {
                Id = Guid.NewGuid(),
                Name = "łerhałs"
            };
            var deleteCommand = new DeleteWarehouseCommand(createCommand.Id);

            // Act
            await _commandsBus.Send(createCommand);
            await _commandsBus.Send(deleteCommand);

            // Assert
            var aggregate = await _aggregateRepository.Load<WarehouseAggregate>(createCommand.Id, 1);

            Assert.AreEqual(createCommand.Name, aggregate.Name);
            Assert.AreEqual(1, aggregate.Version);
            Assert.AreEqual(false, aggregate.IsDeleted);
        }

        [TearDown]
        public async Task Cleanup()
        {
            // TODO: Remove created events
        }
    }
}