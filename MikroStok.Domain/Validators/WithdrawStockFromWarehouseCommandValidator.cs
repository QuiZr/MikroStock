using FluentValidation;
using MikroStok.Domain.Commands;

namespace MikroStok.Domain.Validators
{
    public class WithdrawStockFromWarehouseCommandValidator : AbstractValidator<WithdrawStockFromWarehouseCommand>
    {
        public WithdrawStockFromWarehouseCommandValidator()
        {
            RuleFor(x => x.Count).GreaterThan(0);
        }
    }
}