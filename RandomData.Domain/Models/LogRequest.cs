using RandomData.Domain.SharedInterfaces;

namespace RandomData.Domain.Models
{
    public class LogRequest : IQuery<List<RandomDataAudit>>
    {
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }

    }
}
