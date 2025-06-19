using System.Text.Json.Serialization;

namespace User.Processing.Data
{
    public class UserData
    {
        [JsonPropertyName("data")]
        public DataInfo? Data { get; set; }
        [JsonPropertyName("support")]
        public SupportInfo? Support { get; set; }
    }
}