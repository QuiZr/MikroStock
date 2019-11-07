using System;
using MikroStok.ES.Core.Events;

namespace MikroStok.Domain.Events
{
    public class StockAddedEvent : IEvent
    {
        public StockAddedEvent(Guid id, Guid warehouseId, string productName, int count, int version)
        {
            Id = id;
            WarehouseId = warehouseId;
            ProductName = productName;
            Count = count;
            Version = version;
        }

        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }
        public int Version { get; set; }
    }
}