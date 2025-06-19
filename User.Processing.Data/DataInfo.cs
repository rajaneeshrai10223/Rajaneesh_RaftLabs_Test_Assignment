using System.Text.Json.Serialization;

namespace User.Processing.Data
{
    public class DataInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("first_name")]
        public string? First_Name { get; set; }
        [JsonPropertyName("last_name")]
        public string? Last_Name { get; set; }
        [JsonPropertyName("avatar")]
        public string? Avatar { get; set; }
    }
}
