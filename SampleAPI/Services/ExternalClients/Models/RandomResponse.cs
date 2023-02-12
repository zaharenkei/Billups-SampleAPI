using System.Text.Json.Serialization;

namespace SampleAPI.Services.ExternalClients.Models
{
    public class RandomResponse
    {
        [JsonPropertyName("random_number")]
        public int Value { get; set; }
    }
}
