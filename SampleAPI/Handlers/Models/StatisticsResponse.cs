using SampleAPI.Enums;
using SampleAPI.Services.DataProviders.Models;

namespace SampleAPI.Handlers.Models
{
    public record StatisticsResponse
    {
        public StatisticsResponse(IEnumerable<RoundScore> scores)
        {
            Scores = scores.ToArray();
            var wins = Scores.Count(s => s.Result == GameResult.Win);
            var loses = Scores.Count(s => s.Result == GameResult.Lose);
            var countableGames = wins + loses;
            Score = $"Player: {wins}|{loses} : Computer";
            WinRate = countableGames > 0 ? $"{(float)wins / countableGames:P0}" : $"{0f:P0}";
        }

        public string Score { get; }

        public string WinRate { get; }

        public RoundScore[] Scores { get; }
    }
}
