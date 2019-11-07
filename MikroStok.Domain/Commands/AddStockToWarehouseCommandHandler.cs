using System.Threading.Tasks;
using FluentValidation;
using MikroStok.CQRS.Core.Commands.Interfaces;
using MikroStok.Domain.Aggregates;
using MikroStok.ES.Core;

namespace MikroStok.Domain.Commands
{
    public class AddStockToWarehouseCommandHandler : IHandleCommand<AddStockToWarehouseCommand>
    {
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IValidator<AddStockToWarehouseCommand> _validator;
        
        public AddStockToWarehouseCommandHandler(IAggregateRepository aggregateRepository, 
            IValidator<AddStockToWarehouseCommand> validator)
        {
            _aggregateRepository = aggregateRepository;
            _validator = validator;
        }

        public async Task Handle(AddStockToWarehouseCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
            
            var aggregate = await _aggregateRepository.Load<StockAggregate>(command.Id);
            aggregate.Add(command);
            _aggregateRepository.Store(aggregate);
        }
    }
}