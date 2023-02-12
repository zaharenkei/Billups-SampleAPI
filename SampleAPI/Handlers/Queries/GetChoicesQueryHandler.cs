using SampleAPI.Services.DataProviders;
using SampleAPI.Services.DataProviders.Models;
using SampleAPI.Services.DataProviders.Pagination;

namespace SampleAPI.Handlers.Queries
{
    /// <summary>
    /// Returns the full list of available choices from data provider.
    /// </summary>
    public class GetChoicesQueryHandler : IHandler<IEnumerable<Choice>>
    {
        private readonly IGameChoicesProvider dataProvider;

        public GetChoicesQueryHandler(IGameChoicesProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        /// <summary>
        /// Returns the full list of available choices.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Enumerable of choices.</returns>
        /// <remarks>
        ///     In fact we return only first page there,
        ///     but returning unlimited result set is a bad practice as it could be too big to handle.
        ///     Still, as provided UI couldn't handle pagination and data set is very small,
        ///     i decided not to move it to the controller parameters.
        /// </remarks>
        public async ValueTask<IEnumerable<Choice>> Execute(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var page = await dataProvider.ReadPage(new PageRequest(), token);
            return page.Items;
        }
    }
}
