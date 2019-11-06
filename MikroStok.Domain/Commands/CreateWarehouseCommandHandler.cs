using System.Threading.Tasks;
using FluentValidation;
using MikroStok.CQRS.Core.Commands.Interfaces;
using MikroStok.Domain.Events;
using MikroStok.ES.Core.Events;

namespace MikroStok.Domain.Commands
{
    public class CreateWarehouseCommandHandler : IHandleCommand<CreateWarehouseCommand>
    {
        private readonly IEventBus _eventBus;
        private readonly IValidator<CreateWarehouseCommand> _validator;

        public CreateWarehouseCommandHandler(IValidator<CreateWarehouseCommand> validator, IEventBus eventBus)
        {
            _validator = validator;
            _eventBus = eventBus;
        }

        public async Task Handle(CreateWarehouseCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var e = new WarehouseCreatedEvent(command.Id, command.Name, 1);
            await _eventBus.Publish(e);
        }
    }
}