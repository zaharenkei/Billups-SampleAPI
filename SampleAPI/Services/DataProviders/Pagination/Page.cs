namespace SampleAPI.Services.DataProviders.Pagination
{
    public record Page<TEntity>
    {
        public Page(PageRequest pageRequest, IEnumerable<TEntity> items, int totalRows)
        {
            PageOffset = pageRequest.Offset;
            PageSize = pageRequest.Limit;
            Items = items;
            TotalRows = totalRows;
        }

        public int PageOffset { get; }

        public int PageSize { get; }

        public int TotalRows { get; }

        public IEnumerable<TEntity> Items { get; }
    }
}
