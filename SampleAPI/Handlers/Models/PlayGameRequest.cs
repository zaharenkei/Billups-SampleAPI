using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SampleAPI.Handlers.Models
{
    [ExcludeFromCodeCoverage]
    public class PlayGameRequest
    {
        [JsonPropertyName("player")]
        public int Player { get; set; }

        [JsonIgnore]
        public string? ConnectionId { get; set; }
    }
}
