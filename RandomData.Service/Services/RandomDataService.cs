using Microsoft.Extensions.Logging;
using RandomData.Domain.Models;
using RandomData.Service.Commands.Interfaces;
using RandomData.Service.Queries.Interfaces;
using RandomData.Service.Services.Interface;
using System.Net.Http.Json;
namespace RandomData.Service.Services
{
    public class RandomDataService : IRandomDataService
    {

        private readonly ILogger<RandomDataService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public RandomDataService(ILogger<RandomDataService> logger, IHttpClientFactory httpClientFactory, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher) 
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _commandDispatcher = commandDispatcher;   
            _queryDispatcher = queryDispatcher;
        }

        public async Task<bool> FetchAndSaveRandomData(CancellationToken cancellationToken)
        {
            try
            {
                RandomDataApiResponse? response = null;
                using (var client = _httpClientFactory.CreateClient("RandomDataApi"))
                {
                    response = await client.GetFromJsonAsync<RandomDataApiResponse>("random?auth=null");
                }

                var key = Guid.NewGuid().ToString();
                var responseInfo = new RandomDataAudit
                {
                    PartitionKey = key,
                    Status = true,
                    RequestTime = DateTime.Now,
                };

                if (await _commandDispatcher.DispatchAsync(responseInfo))
                {
                    await _commandDispatcher.DispatchAsync(new RandomDataBlobRequest() { RowKey = responseInfo.RowKey, Data = response });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while Fetching and Saving Random Data: {ex.Message}");
            }
           
            return false;
        }

        public async Task<RandomDataApiResponse?> GetRandomDataFromBlob(string rowkey, CancellationToken cancellationToken = default)
        {
            return await _queryDispatcher.QueryAsync(new RandomDataBlobRequest() { RowKey = rowkey  });
        }

        public async Task<List<RandomDataAudit>> GetLogs(DateTimeOffset FromDate, DateTimeOffset ToDate)
        {
            return await _queryDispatcher.QueryAsync(new LogRequest() { From = FromDate, To = ToDate });
        }
    }
}
