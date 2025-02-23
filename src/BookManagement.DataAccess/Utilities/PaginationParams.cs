namespace BookManagement.DataAccess.Utilities;

public class PaginationParams(int pageIndex, int pageSize)
{
    public int PageIndex { get; set; } = pageIndex < 1 ? 1 : pageIndex;

    public int PageSize { get; set; } = pageSize < 1 ? 10 : pageSize;
}
