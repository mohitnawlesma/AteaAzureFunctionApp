using FluentAssertions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using RandomData.Domain.Models;
using RandomData.Service.Commands.Interfaces;
using RandomData.Service.Queries.Interfaces;
using RandomData.Service.Services;

namespace RandomData.Service.Test.Services
{
    [TestFixture]
    public class RandomDataServiceTest
    {
        public ILogger<RandomDataService> _logger;
        public IHttpClientFactory _httpClientFactory;
        public ICommandDispatcher _commandDispatcher;
        public IQueryDispatcher _queryDispatcher;

        [SetUp]
        public void SetUp()
        {
            _httpClientFactory = Substitute.For<IHttpClientFactory>();
            _logger = Substitute.For<ILogger<RandomDataService>>();
            _commandDispatcher = Substitute.For<ICommandDispatcher>();
            _queryDispatcher = Substitute.For<IQueryDispatcher>();


        }

        [Test]
        public async Task GetLogs_Should_Get_Logs()
        {
            List<RandomDataAudit> data = new List<RandomDataAudit>() { new RandomDataAudit() { Timestamp = DateTimeOffset.Now, Status = true, RequestTime = DateTimeOffset.Now, PartitionKey = "4ac782a9-f456-4eae-b2ac-53798bb37f89", RowKey = "2cc0330f-475c-47ea-aec8-25b7be71e840" } };
            _queryDispatcher.QueryAsync(new LogRequest()).Returns(data);

            var service = new RandomDataService(_logger, _httpClientFactory, _commandDispatcher, _queryDispatcher);
            var result = await service.GetLogs(DateTimeOffset.Now.AddHours(-1), DateTimeOffset.Now);
            result.Count.Should().Be(1);
            result.First().RowKey.Should().Be("2cc0330f-475c-47ea-aec8-25b7be71e840");

        }
    }
}
