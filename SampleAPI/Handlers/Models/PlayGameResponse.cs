using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using SampleAPI.Enums;
using SampleAPI.Services.DataProviders.Models;

namespace SampleAPI.Handlers.Models;

[ExcludeFromCodeCoverage]
public class PlayGameResponse
{
    public PlayGameResponse(RoundScore score)
    :this((int)score.PlayerChoice, (int)score.ComputerChoice, score.Result)
    { }

    public PlayGameResponse(int player, int random, GameResult result)
    {
        PlayerChoice = player;
        ComputerChoice = random;
        Result = result.ToString();
    }

    [JsonPropertyName("player")]
    public int PlayerChoice { get; set; }

    [JsonPropertyName("computer")]
    public int ComputerChoice { get; set; }

    [JsonPropertyName("results")]
    public string Result { get; set; }
}