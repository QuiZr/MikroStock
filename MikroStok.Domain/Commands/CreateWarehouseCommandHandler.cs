using System.Threading.Tasks;
using FluentValidation;
using MikroStok.CQRS.Core.Commands.Interfaces;
using MikroStok.Domain.Aggregates;
using MikroStok.ES.Core;

namespace MikroStok.Domain.Commands
{
    public class CreateWarehouseCommandHandler : IHandleCommand<CreateWarehouseCommand>
    {
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IValidator<CreateWarehouseCommand> _validator;

        public CreateWarehouseCommandHandler(IAggregateRepository aggregateRepository,
            IValidator<CreateWarehouseCommand> validator)
        {
            _aggregateRepository = aggregateRepository;
            _validator = validator;
        }

        public async Task Handle(CreateWarehouseCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var warehouse = new WarehouseAggregate(command);
            _aggregateRepository.Store(warehouse);
        }
    }
}