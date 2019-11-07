using FluentValidation;
using MikroStok.CQRS.Core.Queries.Interfaces;
using MikroStok.Domain.Commands;

namespace MikroStok.Domain.Validators
{
    public class WithdrawStockFromWarehouseCommandValidator : AbstractValidator<WithdrawStockFromWarehouseCommand>
    {
        private readonly IQueryBus _queryBus;
        public WithdrawStockFromWarehouseCommandValidator(IQueryBus queryBus)
        {
            _queryBus = queryBus;
            
            RuleFor(x => x.Count).GreaterThan(0);
        }
    }
}