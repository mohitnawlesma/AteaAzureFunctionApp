using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RandomData.Domain.Models;
using RandomData.Service.Queries.Interfaces;

namespace RandomData.Service.Queries
{
    public class GetDataHandler : IQueryHandler<RandomDataBlobRequest, RandomDataApiResponse>
    {
        private readonly BlobContainerClient _blobContainerClient;
        private readonly ILogger<GetDataHandler> _logger;
        private readonly IConfiguration _configuration;

        public GetDataHandler(ILogger<GetDataHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            var blobStorageClient = new BlobServiceClient(_configuration.GetValue<string>("AzureWebJobsStorage"));
            _blobContainerClient = blobStorageClient.GetBlobContainerClient(_configuration.GetValue<string>("RandomDataBlobName")); 
        }
        public async Task<RandomDataApiResponse?> HandleAsync(RandomDataBlobRequest query)
        {
            try
            {
                var blob = _blobContainerClient.GetBlobClient($"{query.RowKey}.json");
                if (!await blob.ExistsAsync())
                    return null;

                var downloadResult = await blob.DownloadContentAsync();
                return downloadResult.Value.Content.ToObjectFromJson<RandomDataApiResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Getting Data: {ex.Message}");
            }
            return null;
        }
    }
}
