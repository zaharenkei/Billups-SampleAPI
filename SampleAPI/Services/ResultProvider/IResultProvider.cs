using SampleAPI.Enums;
using SampleAPI.Services.DataProviders.Models;

namespace SampleAPI.Services.ResultProvider
{
    /// <summary>
    /// Service for game result calculations
    /// </summary>
    public interface IResultProvider
    {
        /// <summary>
        /// Calculates the result of the game based on the selected choice id's.
        /// </summary>
        /// <param name="player">Player's choice id.</param>
        /// <param name="computer">Computer's choice id.</param>
        /// <returns>GameResult enum member.</returns>
        public RoundScore DefineWinner(int player, int computer);

        /// <summary>
        /// Calculates the result of the game based on the selected choice value.
        /// </summary>
        /// <param name="player">Player's choice value.</param>
        /// <param name="computer">Computer's choice value.</param>
        /// <returns>GameResult enum member.</returns>
        public RoundScore DefineWinner(GameChoice player, GameChoice computer);
    }
}
