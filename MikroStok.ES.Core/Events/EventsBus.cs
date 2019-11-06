using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MikroStok.ES.Core.Events
{
    public class EventsBus : IEventBus
    {
        private readonly Func<Type, IEnumerable<IHandleEvent>> _handlersFactory;
        public EventsBus(Func<Type, IEnumerable<IHandleEvent>> handlersFactory)
        {
            _handlersFactory = handlersFactory;
        }
 
        public async Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlers = _handlersFactory(typeof(TEvent)).Cast<IHandleEvent<TEvent>>();
 
            foreach (var handler in handlers)
            {
                await handler.Handle(@event);
            }
        }
    }
}