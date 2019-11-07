using System;
using MikroStok.CQRS.Core.Queries.Interfaces;

namespace MikroStok.Domain.Queries
{
    public class GetStockInWarehouseQuery : IQuery
    {
        public Guid WarehouseId { get; set; }
    }
}