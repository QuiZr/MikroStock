namespace MikroStok.CQRS.Core.Queries.Interfaces
{
    public interface IQueryBus
    {
        TResult Send<TQuery, TResult>(TQuery query) where TQuery : IQuery;
    }
}