namespace RandomData.Domain.Models
{
    using Azure;
    using Azure.Data.Tables;
    using RandomData.Domain.SharedInterfaces;

    public class RandomDataAudit : ITableEntity, ICommand,IQuery
    {
        public DateTimeOffset? Timestamp { get; set; }
        public bool Status { get; set; } = false;
        public DateTimeOffset RequestTime { get; set; } = DateTimeOffset.Now;
        public string PartitionKey { get; set; } = Guid.NewGuid().ToString();
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public ETag ETag { get; set; }
    }
}
