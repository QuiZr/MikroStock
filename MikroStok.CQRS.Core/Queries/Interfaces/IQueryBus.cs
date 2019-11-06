using System.Threading.Tasks;

namespace MikroStok.CQRS.Core.Queries.Interfaces
{
    public interface IQueryBus
    {
        Task<TResult> Send<TQuery, TResult>(TQuery query) where TQuery : IQuery;
    }
}