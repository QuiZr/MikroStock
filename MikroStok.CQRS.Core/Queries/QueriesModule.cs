using Autofac;
using MikroStok.CQRS.Core.Queries.Interfaces;

namespace MikroStok.CQRS.Core.Queries
{
    public class QueriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.IsAssignableTo<IHandleQuery>())
                .AsImplementedInterfaces();

            builder.RegisterType<QueryBus>()
                .AsImplementedInterfaces();
        }
    }
}