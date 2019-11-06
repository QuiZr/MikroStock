using System.Threading.Tasks;

namespace MikroStok.ES.Core.Events
{
    public interface IHandleEvent<TEvent> : IHandleEvent
        where TEvent : IEvent
    {
        Task Handle(TEvent e);
    }

    public interface IHandleEvent
    {
    }
}