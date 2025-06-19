using System.Text.Json.Serialization;

namespace User.Processing.Data
{
    public class DataInfoWithPaging
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("per_page")]
        public int PerPage { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
        
        [JsonPropertyName("data")]
        public IEnumerable<DataInfo>? Data { get; set; }

        [JsonPropertyName("support")]
        public SupportInfo? Support { get; set; }
    }
}
