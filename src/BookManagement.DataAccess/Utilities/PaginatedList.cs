namespace BookManagement.DataAccess.Utilities;

public class PaginatedList<T>
{
    public PaginatedList(
        List<T> items,
        int pageIndex,
        int pageSize,
        int totalCount)
    {
        Items = items;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(TotalCount / (float)PageSize);
    }

    public List<T> Items { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }
}
