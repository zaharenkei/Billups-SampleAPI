using SampleAPI.Enums;

namespace SampleAPI.Services.DataProviders.Models
{
    public record RoundScore(GameChoice PlayerChoice, GameChoice ComputerChoice, GameResult Result)
    {
        public RoundScore(int player, int computer, GameResult result)
        : this((GameChoice)player, (GameChoice)computer, result)
        { }
    }
}
