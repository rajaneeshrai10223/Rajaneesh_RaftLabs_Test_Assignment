using System.Text.Json.Serialization;

namespace User.Processing.Data
{
    public class SupportInfo
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
