using SampleAPI.Infrastructure.Exceptions;

namespace SampleAPI.Services.DataProviders
{
    /// <summary>
    /// Base abstraction around the data set which specifies the read-only access.
    /// </summary>
    /// <typeparam name="TId">Id that will be used to retrieve the entity.</typeparam>
    /// <typeparam name="TEntity">Entity that is contained in that data set.</typeparam>
    public interface IReadOnlyDataProvider<in TId, TEntity>
    {
        /// <summary>
        /// Checks if the entity with specified id Exists in the data set.
        /// </summary>
        /// <param name="id">Id of the entity.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>True if element was found.</returns>
        public ValueTask<bool> Exist(TId id, CancellationToken token);

        /// <summary>
        /// Counts the entire available entities set.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Amount of available entities.</returns>
        public ValueTask<int> Count(CancellationToken token);

        /// <summary>
        /// Reads an element by id.
        /// </summary>
        /// <param name="id">Id of the element to be read.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Entity that was read.</returns>
        /// <exception cref="DataProviderException">Throws if the specified entity could not be found.</exception>
        public ValueTask<TEntity> Read(TId id, CancellationToken token);
    }
}
