namespace RandomData.Domain.Models
{
    public class RandomDataApiResponse
    {
        public int count { get; set; }
        public List<RandomDataEntryDetail>? entries { get; set; }
    }
}
