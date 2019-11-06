using Autofac;
using FluentValidation;
using MikroStok.CQRS.Core.Commands.Interfaces;

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
        }
    }
}