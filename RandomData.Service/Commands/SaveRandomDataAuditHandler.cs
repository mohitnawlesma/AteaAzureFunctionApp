using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RandomData.Domain.Models;
using RandomData.Service.Commands.Interfaces;

namespace RandomData.Service.Commands
{
    internal class SaveRandomDataAuditHandler : ICommandHandler<RandomDataAudit>
    {
        private readonly TableClient _tableClient;
        private readonly ILogger<SaveRandomDataAuditHandler> _logger;
        private readonly IConfiguration _configuration;

        public SaveRandomDataAuditHandler(ILogger<SaveRandomDataAuditHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration; 
            _tableClient = new TableClient(_configuration.GetValue<string>("AzureWebJobsStorage"), _configuration.GetValue<string>("RandomDataAuditName"));
        }
        public async Task<bool> HandleAsync(RandomDataAudit command)
        {
            try
            {
                await _tableClient.CreateIfNotExistsAsync();
                await _tableClient.AddEntityAsync(command);
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
