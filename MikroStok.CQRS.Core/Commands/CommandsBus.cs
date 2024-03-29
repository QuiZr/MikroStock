using System;
using System.Threading.Tasks;
using MikroStok.CQRS.Core.Commands.Interfaces;

namespace MikroStok.CQRS.Core.Commands
{
    public class CommandsBus : ICommandsBus
    {
        private readonly Func<Type, IHandleCommand> _handlersFactory;

        public CommandsBus(Func<Type, IHandleCommand> handlersFactory)
        {
            _handlersFactory = handlersFactory;
        }

        public async Task Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = (IHandleCommand<TCommand>) _handlersFactory(typeof(TCommand));
            await handler.Handle(command);
        }
    }
}