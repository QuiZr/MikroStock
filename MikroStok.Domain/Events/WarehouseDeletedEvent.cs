using System;
using MikroStok.ES.Core.Events;

namespace MikroStok.Domain.Events
{
    public class WarehouseDeletedEvent : IEvent
    {
        public WarehouseDeletedEvent(Guid id, int version)
        {
            Id = id;
            Version = version;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}