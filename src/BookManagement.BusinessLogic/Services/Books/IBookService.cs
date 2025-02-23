using BookManagement.BusinessLogic.Common;
using BookManagement.BusinessLogic.Models.Books;
using BookManagement.DataAccess.Utilities;

namespace BookManagement.BusinessLogic.Services.Books;

public interface IBookService
{
    Task<Result<long>> AddAsync(BookCreateModel model);

    Task<Result> AddBulkAsync(List<BookCreateModel> models);

    Task<Result<BookResultModel>> GetByIdAsync(long id);

    Task<Result<BookResultModel>> GetByTitleAsync(string title);

    Task<Result<PaginatedList<string>>> GetTitlesPopularityAsync(PaginationParams paginationParams);

    Task<Result<PaginatedList<string>>> GetSoftDeletedBookTitlesAsync(PaginationParams paginationParams);

    Task<Result<long>> UpdateAync(long id, BookUpdateModel model);

    Task<Result> SoftDeleteAsync(long id);

    Task<Result> SoftDeleteBulkAsync(List<long> ids);

    Task<Result> RestoreByTitleAsync(string title);
}
