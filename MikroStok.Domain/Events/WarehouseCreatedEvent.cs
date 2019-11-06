using System;

namespace MikroStok.Domain.Events
{
    public class WarehouseCreatedEvent
    {
        public WarehouseCreatedEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}