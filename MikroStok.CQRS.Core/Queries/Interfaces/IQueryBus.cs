using System.Threading.Tasks;

namespace MikroStok.CQRS.Core.Queries.Interfaces
{
    public interface IQueryBus
    {
        Task<TResult> Query<TQuery, TResult>(TQuery query) where TQuery : IQuery;
    }
}