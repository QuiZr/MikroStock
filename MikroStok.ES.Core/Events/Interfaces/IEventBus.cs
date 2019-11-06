using System.Threading.Tasks;

namespace MikroStok.ES.Core.Events
{
    public interface IEventBus
    {
        Task Publish<TEvent>(TEvent command) where TEvent : IEvent;
    }
}