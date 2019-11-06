using System.Threading.Tasks;

namespace MikroStok.CQRS.Core.Commands.Interfaces
{
    public interface IHandleCommand<TCommand> : IHandleCommand
        where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }

    public interface IHandleCommand
    {
    }
}