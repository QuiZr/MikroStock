using System;
using MikroStok.ES.Core.Events;

namespace MikroStok.Domain.Events
{
    public class WarehouseCreatedEvent : IEvent
    {
        public WarehouseCreatedEvent(Guid id, string name, int version)
        {
            Id = id;
            Name = name;
            Version = version;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
    }
}