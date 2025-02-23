using BookManagement.DataAccess.Contexts;
using BookManagement.DataAccess.Extensions;
using BookManagement.DataAccess.Infrastructure.Calculators;
using BookManagement.DataAccess.Infrastructure.Clock;
using BookManagement.DataAccess.Repositories.Common;
using BookManagement.DataAccess.Utilities;
using BookManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookManagement.DataAccess.Repositories.Books;

public class BookRepository(
    ApplicationDbContext context,
    IDateTimeProvider dateTimeProvider)
    : Repository<Book>(context, dateTimeProvider), IBookRepository
{
    public async Task<PaginatedList<string>> GetTitlesAsync(PaginationParams paginationParams)
    {
        int currentYear = DateTime.UtcNow.Year;
        return await dbSet
            .AsNoTracking()
            .OrderByDescending(BookPopularityCalculator.GetCalculateExpressiom(currentYear))
            .Select(book => book.Title)
            .ToPaginatedListAsync(paginationParams);
    }

    public async Task<PaginatedList<string>> GetSoftDeletedTitlesAsync(PaginationParams paginationParams)
    {
        int currentYear = DateTime.UtcNow.Year;
        return await dbSet
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(book => book.IsDeleted)
            .OrderByDescending(BookPopularityCalculator.GetCalculateExpressiom(currentYear))
            .Select(book => book.Title)
            .ToPaginatedListAsync(paginationParams);
    }

    public async Task IncrementViewsAsync(Expression<Func<Book, bool>> expression)
    {
        await dbSet
            .Where(expression)
            .ExecuteUpdateAsync(book => book.SetProperty(book => book.Views, book => book.Views + 1));
    }

    public async Task<bool> IsExistAsync(Expression<Func<Book, bool>> expression)
    {
        return await dbSet.AnyAsync(expression);
    }

    public async Task<bool> RestoreAsync(Expression<Func<Book, bool>> expression)
    {
        var affectedRows = await dbSet
            .IgnoreQueryFilters()
            .Where(book => book.IsDeleted)
            .Where(expression)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.IsDeleted, false)
                .SetProperty(b => b.DeletedOnUtc, (DateTime?)null)
        );

        return affectedRows > 0;
    }
}