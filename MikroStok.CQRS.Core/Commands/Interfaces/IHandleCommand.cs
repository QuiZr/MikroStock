namespace MikroStok.CQRS.Core.Commands.Interfaces
{
    public interface IHandleCommand<TCommand> : IHandleCommand
        where TCommand : ICommand
    {
        void Handle(TCommand command);
    }

    public interface IHandleCommand
    {
    }
}