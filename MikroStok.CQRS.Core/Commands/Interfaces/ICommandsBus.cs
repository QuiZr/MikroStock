using System.Threading.Tasks;

namespace MikroStok.CQRS.Core.Commands.Interfaces
{
    public interface ICommandsBus
    {
        Task Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}