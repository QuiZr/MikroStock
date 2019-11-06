namespace MikroStok.CQRS.Core.Commands.Interfaces
{
    public interface ICommandsBus
    {
        void Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}