using SampleAPI.Enums;
using SampleAPI.Infrastructure.Exceptions;
using SampleAPI.Services.DataProviders.Models;
using SampleAPI.Services.DataProviders.Pagination;

namespace SampleAPI.Services.DataProviders
{
    /// <inheritdocs/>
    public class GameChoicesProvider : IGameChoicesProvider
    {
        /// <inheritdocs/>
        public ValueTask<bool> Exist(int id, CancellationToken token)
        {
            var exist = Enum.IsDefined(typeof(GameChoice), id);
            return ValueTask.FromResult(exist);
        }

        /// <inheritdocs/>
        public ValueTask<int> Count(CancellationToken token)
        {
            var length = Enum.GetNames<GameChoice>().Length;
            return ValueTask.FromResult(length);
        }

        /// <inheritdocs/>
        public ValueTask<Choice> Read(int id, CancellationToken token)
        {
            var name = Enum.GetName(typeof(GameChoice), id);
            if (name == null)
            {
                throw new DataProviderException($"Element with id {id} wasn't found.");
            }

            return ValueTask.FromResult(new Choice(id, name));
        }

        /// <inheritdocs/>
        public ValueTask<Page<Choice>> ReadPage(PageRequest request, CancellationToken token)
        {
            var totalRows = Enum.GetNames<GameChoice>().Length;
            var items = Enum.GetValues<GameChoice>()
                .OrderBy(item => (int)item)
                .Skip(request.Offset).Take(request.Limit)
                .Select(element => new Choice(element));
            var page = new Page<Choice>(request, items, totalRows);

            return ValueTask.FromResult(page);
        }
    }
}
