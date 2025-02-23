using BookManagement.DataAccess.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.DataAccess.Extensions;

public static class PaginationExtensions
{
    public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> query, PaginationParams paginationParams)
    {
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((paginationParams.PageIndex - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        return new PaginatedList<T>(items, paginationParams.PageIndex, paginationParams.PageSize, totalCount);
    }
}
