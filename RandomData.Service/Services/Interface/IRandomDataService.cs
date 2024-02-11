using RandomData.Domain.Models;

namespace RandomData.Service.Services.Interface
{
    public interface IRandomDataService
    {
        Task<bool> FetchAndSaveRandomData(CancellationToken cancellationToken);
        Task<RandomDataApiResponse?> GetRandomDataFromBlob(string rowkey, CancellationToken cancellationToken = default);
        Task<List<RandomDataAudit>> GetLogs(DateTimeOffset FromDate, DateTimeOffset ToDate);
    }
}
