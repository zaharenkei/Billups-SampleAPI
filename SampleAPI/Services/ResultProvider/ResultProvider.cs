using SampleAPI.Enums;
using SampleAPI.Services.DataProviders.Models;

namespace SampleAPI.Services.ResultProvider;

/// <inheritdocs/>
public class ResultProvider : IResultProvider
{
    private readonly Dictionary<GameChoice, IEnumerable<GameChoice>> victoryTable;

    /// <summary>
    /// Initializes the provider with data matching set.
    /// </summary>
    public ResultProvider()
    {
        victoryTable = new Dictionary<GameChoice, IEnumerable<GameChoice>>
        {
            { GameChoice.Rock, new[] {GameChoice.Scissors, GameChoice.Lizard}},
            { GameChoice.Paper, new[] {GameChoice.Rock, GameChoice.Spock}},
            { GameChoice.Scissors, new[] {GameChoice.Paper, GameChoice.Lizard}},
            { GameChoice.Lizard, new[] {GameChoice.Rock, GameChoice.Spock}},
            { GameChoice.Spock, new[] {GameChoice.Rock, GameChoice.Scissors}}
        };
    }

    /// <inheritdocs/>
    public RoundScore DefineWinner(int player, int computer)
    {
        return DefineWinner((GameChoice)player, (GameChoice)computer);
    }

    /// <inheritdocs/>
    public RoundScore DefineWinner(GameChoice player, GameChoice computer)
    {
        if (player == computer)
            return new RoundScore(player, computer, GameResult.Tie);

        return victoryTable[player].Contains(computer)
            ? new RoundScore(player, computer, GameResult.Win)
            : new RoundScore(player, computer, GameResult.Lose);
    }
}