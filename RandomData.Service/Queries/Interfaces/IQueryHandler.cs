using RandomData.Domain.SharedInterfaces;

namespace RandomData.Service.Queries.Interfaces
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : class, IQuery
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
