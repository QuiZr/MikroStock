using System.Threading.Tasks;
using FluentValidation;
using MikroStok.CQRS.Core.Commands.Interfaces;
using MikroStok.Domain.Aggregates;
using MikroStok.ES.Core;

namespace MikroStok.Domain.Commands
{
    public class WithdrawStockFromWarehouseCommandHandler : IHandleCommand<WithdrawStockFromWarehouseCommand>
    {
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IValidator<WithdrawStockFromWarehouseCommand> _validator;

        public WithdrawStockFromWarehouseCommandHandler(IAggregateRepository aggregateRepository, 
            IValidator<WithdrawStockFromWarehouseCommand> validator)
        {
            _aggregateRepository = aggregateRepository;
            _validator = validator;
        }

        public async Task Handle(WithdrawStockFromWarehouseCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
            
            var aggregate = await _aggregateRepository.Load<StockAggregate>(command.Id, command.Version);
            if (aggregate == null || aggregate.Version == 0)
            {
                throw new ValidationException("No stock with provided ID exists");
            }
            aggregate.Withdraw(command);
            _aggregateRepository.Store(aggregate);
        }
    }
}