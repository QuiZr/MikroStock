using System;
using System.Collections.Generic;
using Autofac;

namespace MikroStok.ES.Core.Events
{
    public class EventsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
 
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.IsAssignableTo<IHandleEvent>())
                .AsImplementedInterfaces();
            
            builder.RegisterGeneric(typeof (AllEventsHandler<>))
                .As(typeof (IHandleEvent<>));
            
            builder.Register<Func<Type, IEnumerable<IHandleEvent>>>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();
                return t =>
                {
                    var handlerType = typeof(IHandleEvent<>).MakeGenericType(t);
                    var handlersCollectionType = typeof(IEnumerable<>).MakeGenericType(handlerType);
                    return (IEnumerable<IHandleEvent>)ctx.Resolve(handlersCollectionType);
                };
            });
 
            builder.RegisterType<EventsBus>()
                .AsImplementedInterfaces();
        }
    }
}