using System;
using MikroStok.ES.Core.Events;

namespace MikroStok.Domain.Events
{
    public class StockWithdrawnEvent : IEvent
    {
        public StockWithdrawnEvent(Guid id, int version, int count)
        {
            Id = id;
            Version = version;
            Count = count;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public int Count { get; set; }
    }
}