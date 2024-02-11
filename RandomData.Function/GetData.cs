using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using RandomData.Service.Services.Interface;

namespace RandomData.Function
{
    public class GetData
    {
        private readonly IRandomDataService _service;
        private readonly ILogger<SaveData> _logger;

        public GetData(ILogger<SaveData> logger, IRandomDataService service)
        {
            _service = service;
            _logger = logger;
        }

        [Function("GetData")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "responses/{id}")] HttpRequestData req, string id, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _service.GetRandomDataFromBlob(id, cancellationToken);
                if (data == null)
                {
                    return req.CreateResponse(HttpStatusCode.NotFound);
                }

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(data, cancellationToken);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Fetching Data : {ex.Message}");
            }
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
