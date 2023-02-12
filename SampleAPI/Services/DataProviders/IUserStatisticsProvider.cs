using System.Security.Cryptography;
using SampleAPI.Services.DataProviders.Models;

namespace SampleAPI.Services.DataProviders
{
    public interface IUserStatisticsProvider : IReadOnlyDataProvider<string, IEnumerable<RoundScore>>
    {
        /// <summary>
        /// Creates or updates the statistics for correspondent Id.
        /// </summary>
        /// <param name="id">Id of the element to be processed.</param>
        /// <param name="score">Score to be added to the statistics.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Statistics set.</returns>
        public ValueTask<IEnumerable<RoundScore>> CreateOrUpdate(string id, RoundScore score, CancellationToken token);

        /// <summary>
        /// Deletes all statistics referred to Id.
        /// </summary>
        /// <param name="id">Id of the element to be cleared.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>True if entity was deleted successfully or no entity found.</returns>
        public ValueTask<bool> Delete(string id, CancellationToken token);
    }
}
