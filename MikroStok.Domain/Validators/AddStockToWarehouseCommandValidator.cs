using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MikroStok.CQRS.Core.Queries.Interfaces;
using MikroStok.Domain.Commands;
using MikroStok.Domain.Models;
using MikroStok.Domain.Queries;

namespace MikroStok.Domain.Validators
{
    public class AddStockToWarehouseCommandValidator : AbstractValidator<AddStockToWarehouseCommand>
    {
        private readonly IQueryBus _queryBus;
        public AddStockToWarehouseCommandValidator(IQueryBus queryBus)
        {
            _queryBus = queryBus;
            
            RuleFor(x => x.Count).GreaterThanOrEqualTo(0);
            RuleFor(x => x.WarehouseId).MustAsync(BeValidWarehouseId);
        }

        private async Task<bool> BeValidWarehouseId(Guid warehouseId, CancellationToken ct)
        {
            var warehouses = await _queryBus.Query<GetWarehousesQuery, IReadOnlyList<Warehouse>>(new GetWarehousesQuery());
            return warehouses.Any(x => x.Id == warehouseId);
        }
    }
}