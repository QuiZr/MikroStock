using System;

namespace MikroStok.ES.Core.Events
{
    public interface IEvent
    {
        Guid Id { get; set; }
        int Version { get; set; }
    }
}