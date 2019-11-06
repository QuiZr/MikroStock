using FluentValidation;
using MikroStok.Domain.Commands;

namespace MikroStok.Domain.Validators
{
    public class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
    {
        public CreateWarehouseCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}