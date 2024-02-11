using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RandomData.Domain.Models;
using RandomData.Service.Commands.Interfaces;

namespace RandomData.Service.Commands
{
    public class SaveDataToBlobHandler : ICommandHandler<RandomDataBlobRequest>
    {
        private readonly BlobContainerClient _blobContainerClient;
        private readonly ILogger<SaveDataToBlobHandler> _logger;
        private readonly IConfiguration _configuration;


        public SaveDataToBlobHandler(ILogger<SaveDataToBlobHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            var blobStorageClient = new BlobServiceClient(_configuration.GetValue<string>("AzureWebJobsStorage"));
            _blobContainerClient = blobStorageClient.GetBlobContainerClient(_configuration.GetValue<string>("RandomDataBlobName"));

        }
        public async Task<bool> HandleAsync(RandomDataBlobRequest command)
        {
            try
            {               
                await _blobContainerClient.CreateIfNotExistsAsync();
                await _blobContainerClient.UploadBlobAsync($"{command.RowKey}.json", BinaryData.FromString(JsonConvert.SerializeObject(command.Data)));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error storing Audit Data: {ex.Message}");
            }
            return false;
        }
    }
}
