using RandomData.Domain.SharedInterfaces;

namespace RandomData.Service.Queries.Interfaces
{
    public interface IQueryDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}
