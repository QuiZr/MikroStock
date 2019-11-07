using System;
using Autofac;
using FluentValidation;
using MikroStok.CQRS.Core.Commands.Interfaces;
using MikroStok.CQRS.Core.Queries.Interfaces;

namespace MikroStok.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.IsAssignableTo<IHandleCommand>())
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.IsAssignableTo<IValidator>())
                .AsImplementedInterfaces();
            
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.IsAssignableTo<IHandleQuery>())
                .AsImplementedInterfaces();
            
            builder.Register<Func< (Type, Type), IHandleQuery>>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();

                return t =>
                {
                    var handlerType = typeof(IHandleQuery<,>).MakeGenericType(t.Item1, t.Item2);
                    return (IHandleQuery) ctx.Resolve(handlerType);
                };
            });
        }
    }
}