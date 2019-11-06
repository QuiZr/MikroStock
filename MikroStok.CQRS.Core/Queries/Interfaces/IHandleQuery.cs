namespace MikroStok.CQRS.Core.Queries.Interfaces
{
    public interface IHandleQuery<TQuery, TResult> : IHandleQuery
        where TQuery : IQuery
    {
        TResult Handle(TQuery query);
    }

    public interface IHandleQuery
    {
    }
}