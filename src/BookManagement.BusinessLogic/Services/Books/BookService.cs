using AutoMapper;
using BookManagement.BusinessLogic.Common;
using BookManagement.BusinessLogic.Errors;
using BookManagement.BusinessLogic.Extensions;
using BookManagement.BusinessLogic.Models.Books;
using BookManagement.DataAccess.Contexts;
using BookManagement.DataAccess.Infrastructure.Calculators;
using BookManagement.DataAccess.Infrastructure.Clock;
using BookManagement.DataAccess.Repositories.Books;
using BookManagement.DataAccess.Utilities;
using BookManagement.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.BusinessLogic.Services.Books;

public class BookService(
    IMapper mapper,
    IBookRepository repository,
    IValidator<BookCreateModel> createValidator,
    IValidator<BookUpdateModel> updateValidator,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork) : IBookService
{
    public async Task<Result<long>> AddAsync(BookCreateModel model)
    {
        var validationResult = await createValidator.ValidateModelsAsync(model);
        if (validationResult.IsFailure)
            return Result.Failure<long>(validationResult.Error);

        var doesBookExist = await repository.IsExistAsync(book => book.Title == model.Title);
        if (doesBookExist)
            return Result.Failure<long>(BookErrors.DuplicateTitle());

        var book = mapper.Map<Book>(model);
        await repository.AddAsync(book);
        await unitOfWork.SaveChangesAsync();

        return book.Id;
    }

    public async Task<Result> AddBulkAsync(List<BookCreateModel> models)
    {
        var validationResult = await createValidator.ValidateModelsAsync(models.ToArray());
        if (validationResult.IsFailure)
            return Result.Failure(validationResult.Error);

        var modelTitles = models.Select(model => model.Title).ToList();

        var existingTitles = await repository
            .GetAll()
            .Where(book => modelTitles.Contains(book.Title))
            .Select(book => book.Title)
            .ToListAsync();

        var newBooks = models
            .FindAll(model => !existingTitles.Contains(model.Title));

        if (newBooks.Count == 0)
            return Result.Failure(BookErrors.DuplicateTitles(existingTitles));

        var books = mapper.Map<IEnumerable<Book>>(newBooks);
        await repository.AddRangeAsync(books);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<BookResultModel>> GetByIdAsync(long id)
    {
        var book = await repository.GetAsync(book => book.Id == id);
        if (book is null)
            return Result.Failure<BookResultModel>(BookErrors.NotFound(id));

        await repository.IncrementViewsAsync(book => book.Id == id);
        await unitOfWork.SaveChangesAsync();

        var bookResultModel = mapper.Map<BookResultModel>(book);
        bookResultModel.Views++;
        bookResultModel.PopularityScore = BookPopularityCalculator
            .Calculate(bookResultModel.Views, bookResultModel.PublicationYear, dateTimeProvider.UtcNow.Year);

        return bookResultModel;
    }

    public async Task<Result<BookResultModel>> GetByTitleAsync(string title)
    {
        var book = await repository.GetAsync(book => book.Title == title);
        if (book is null)
            return Result.Failure<BookResultModel>(BookErrors.NotFoundByTitle(title));

        await repository.IncrementViewsAsync(book => book.Title == title);
        await unitOfWork.SaveChangesAsync();

        var bookResultModel = mapper.Map<BookResultModel>(book);
        bookResultModel.Views++;
        bookResultModel.PopularityScore = BookPopularityCalculator
            .Calculate(bookResultModel.Views, bookResultModel.PublicationYear, dateTimeProvider.UtcNow.Year);

        return bookResultModel;
    }

    public async Task<Result<PaginatedList<string>>> GetTitlesPopularityAsync(PaginationParams paginationParams)
    {
        return await repository.GetTitlesAsync(paginationParams);
    }

    public async Task<Result<long>> UpdateAync(long id, BookUpdateModel model)
    {
        var validationResult = await updateValidator.ValidateModelsAsync(model);
        if (validationResult.IsFailure)
            return Result.Failure<long>(validationResult.Error);

        var doesBookExist = await repository.IsExistAsync(book
            => book.Title == model.Title && book.Id != id);
        if (doesBookExist)
            return Result.Failure<long>(BookErrors.DuplicateTitle());

        var book = await repository.GetAsync(book => book.Id == id);
        if (book is null)
            return Result.Failure<long>(BookErrors.NotFound(id));

        mapper.Map(model, book);
        await repository.UpdateAsync(book);
        await unitOfWork.SaveChangesAsync();

        return book.Id;
    }

    public async Task<Result> SoftDeleteAsync(long id)
    {
        var book = await repository.GetAsync(book => book.Id == id);
        if (book is null)
            return Result.Failure(BookErrors.NotFound(id));

        await repository.RemoveAsync(book);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> SoftDeleteBulkAsync(List<long> ids)
    {
        var books = await repository
            .GetAll()
            .Where(book => ids.Contains(book.Id))
            .ToListAsync();

        if (books.Count == 0)
            return Result.Failure(BookErrors.NotFoundAny());

        await repository.RemoveRangeAsync(books);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
