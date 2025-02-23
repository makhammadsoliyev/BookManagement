using BookManagement.DataAccess.Repositories.Common;
using BookManagement.DataAccess.Utilities;
using BookManagement.Domain.Entities;
using System.Linq.Expressions;

namespace BookManagement.DataAccess.Repositories.Books;

public interface IBookRepository : IRepository<Book>
{
    Task<bool> IsExistAsync(Expression<Func<Book, bool>> expression);

    Task<PaginatedList<string>> GetTitlesAsync(PaginationParams paginationParams);

    Task<PaginatedList<string>> GetSoftDeletedTitlesAsync(PaginationParams paginationParams);

    Task IncrementViewsAsync(Expression<Func<Book, bool>> expression);

    Task<bool> RestoreAsync(Expression<Func<Book, bool>> expression);
}
