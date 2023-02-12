namespace SampleAPI.Services.DataProviders.Pagination;

public record PageRequest(int Offset, int Limit)
{
    public PageRequest()
        : this(Pagination.DefaultOffset, Pagination.DefaultLimit)
    { }
}