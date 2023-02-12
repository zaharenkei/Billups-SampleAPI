using SampleAPI.Services.DataProviders.Models;
using SampleAPI.Services.DataProviders.Pagination;

namespace SampleAPI.Services.DataProviders
{
    public interface IGameChoicesProvider : IReadOnlyDataProvider<int, Choice>
    {
        /// <summary>
        /// Reads a page of elements from a data set.
        /// </summary>
        /// <param name="pageRequest">Pagination parameters.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Page of requested entities.</returns>
        ValueTask<Page<Choice>> ReadPage(PageRequest pageRequest, CancellationToken token);
    }
}
