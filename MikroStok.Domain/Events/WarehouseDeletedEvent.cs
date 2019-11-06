using System;

namespace MikroStok.Domain.Events
{
    public class WarehouseDeletedEvent
    {
        public WarehouseDeletedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}