using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RandomData.Domain.Models;
using RandomData.Service.Queries.Interfaces;

namespace RandomData.Service.Queries
{
    public class GetLogsHandler : IQueryHandler<LogRequest, List<RandomDataAudit>>
    {
        private readonly TableClient _tableClient;
        private readonly ILogger<GetLogsHandler> _logger;
        private readonly IConfiguration _configuration;

        public GetLogsHandler(ILogger<GetLogsHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _tableClient = new TableClient(_configuration.GetValue<string>("AzureWebJobsStorage"), _configuration.GetValue<string>("RandomDataAuditName"));
        }
        public async Task<List<RandomDataAudit>> HandleAsync(LogRequest query)
        {
            try
            {
                var queryAsync = _tableClient.QueryAsync<RandomDataAudit>(responseInfo =>
                responseInfo.RequestTime >= query.From && responseInfo.RequestTime < query.To);

                return await queryAsync.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Getting logs: {ex.Message}");
            }
            return new();
        }
    }
}
