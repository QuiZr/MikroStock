using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MikroStok.ES.Core
{
    // Infrastructure to capture modifications to state in events
    public abstract class AggregateBase
    {
        // JsonIgnore - for making sure that it won't be stored in inline projection
        [JsonIgnore] private readonly List<object> _uncommittedEvents = new List<object>();

        // For indexing our event streams
        public Guid Id { get; protected set; }

        // For protecting the state, i.e. conflict prevention
        public int Version { get; protected set; }

        // Get the deltas, i.e. events that make up the state, not yet persisted
        public IEnumerable<object> GetUncommittedEvents()
        {
            return _uncommittedEvents;
        }

        // Mark the deltas as persisted.
        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        protected void AddUncommittedEvent(object @event)
        {
            // add the event to the uncommitted list
            _uncommittedEvents.Add(@event);
        }
    }
}