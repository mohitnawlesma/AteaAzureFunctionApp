using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RandomData.Service.Services.Interface;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace RandomData.Function
{
    public class GetLogs
    {
        private readonly IRandomDataService _service;
        private readonly ILogger<SaveData> _logger;

        public GetLogs(ILogger<SaveData> logger, IRandomDataService service)
        {
            _service = service;
            _logger = logger;
        }

        [Function("GetLogs")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req, CancellationToken cancellationToken)
        {
            try
            {
                var from = req.Query["from"] is not null ? DateTimeOffset.Parse(req.Query["from"]) : DateTimeOffset.Now.AddHours(-1);
                var to = req.Query["to"] is not null ? DateTimeOffset.Parse(req.Query["to"]) : DateTimeOffset.Now;
                var result = await _service.GetLogs(from, to);
                if (result == null || (result is not null && !result.Any()))
                {
                    return req.CreateResponse(HttpStatusCode.NotFound);
                }
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(result, cancellationToken);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Fetching Data : {ex.Message}");
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }            
        }
    }
}
