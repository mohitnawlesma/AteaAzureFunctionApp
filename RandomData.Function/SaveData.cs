using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RandomData.Service.Services.Interface;

namespace RandomData.Function
{
    public class SaveData
    {
        private readonly IRandomDataService _service;
        private readonly ILogger<SaveData> _logger;

        public SaveData(ILogger<SaveData> logger,IRandomDataService service)
        {
            _service = service;
            _logger = logger;
        }

        [Function("FetchData")]
        public async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, CancellationToken cancellationToken)
        {
            try
            {
                await _service.FetchAndSaveRandomData(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Fetching Data : {ex.Message}");
            }

        }
    }
}
