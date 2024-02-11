using RandomData.Domain.SharedInterfaces;

namespace RandomData.Domain.Models
{
    public class RandomDataBlobRequest : ICommand, IQuery<RandomDataApiResponse>
    {
        public string? RowKey { get; set; }
        public RandomDataApiResponse? Data { get; set; }

    }
}
