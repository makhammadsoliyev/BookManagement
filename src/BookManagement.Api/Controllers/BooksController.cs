using BookManagement.Api.Extensions;
using BookManagement.BusinessLogic.Models.Books;
using BookManagement.BusinessLogic.Services.Books;
using BookManagement.DataAccess.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Api.Controllers;

/// <summary>
/// Controller responsible for managing books.
/// </summary>
[ApiController]
[Route("api/books")]
[Authorize(Roles = "User, Admin")]
public class BooksController(IBookService bookService) : ControllerBase
{
    /// <summary>
    /// Adds a new book.
    /// </summary>
    /// <param name="model">The book creation model containing book details.</param>
    /// <returns>A response indicating the success or failure of the operation.</returns>
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] BookCreateModel model)
    {
        var result = await bookService.AddAsync(model);

        return this.ToIActionResult(result);
    }

    /// <summary>
    /// Adds multiple books in bulk.
    /// </summary>
    /// <param name="models">A list of book creation models.</param>
    /// <returns>A response indicating the success or failure of the operation.</returns>
    [HttpPost("bulk")]
    public async Task<IActionResult> AddBulkAsync(
        [FromBody] List<BookCreateModel> models)
    {
        var result = await bookService.AddBulkAsync(models);

        return this.ToIActionResult(result);
    }

    /// <summary>
    /// Retrieves a book by its unique identifier.
    /// </summary>
    /// <param name="id">The book ID.</param>
    /// <returns>The requested book details.</returns>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] long id)
    {
        var result = await bookService.GetByIdAsync(id);

        return this.ToIActionResult(result);
    }

    /// <summary>
    /// Retrieves a book by its title.
    /// </summary>
    /// <param name="title">The book title.</param>
    /// <returns>The requested book details.</returns>
    [HttpGet("{title}")]
    public async Task<IActionResult> GetByTitleAsync([FromRoute] string title)
    {
        var result = await bookService.GetByTitleAsync(title);

        return this.ToIActionResult(result);
    }

    /// <summary>
    /// Retrieves paginated book titles sorted by popularity.
    /// </summary>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="pageIndex">The page index.</param>
    /// <returns>A paginated list of book titles.</returns>
    [HttpGet]
    public async Task<IActionResult> GetTitlesAsync(
        [FromQuery] int pageSize,
        [FromQuery] int pageIndex)
    {
        var result = await bookService
            .GetTitlesPopularityAsync(new PaginationParams(pageIndex, pageSize));

        return this.ToIActionResult(result);
    }

    /// <summary>
    /// Updates an existing book.
    /// </summary>
    /// <param name="id">The book ID.</param>
    /// <param name="model">The book update model.</param>
    /// <returns>A response indicating the success or failure of the update.</returns>
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] long id,
        [FromBody] BookUpdateModel model)
    {
        var result = await bookService.UpdateAync(id, model);

        return this.ToIActionResult(result);
    }

    /// <summary>
    /// Deletes a book by its unique identifier.
    /// </summary>
    /// <param name="id">The book ID.</param>
    /// <returns>A response indicating the success or failure of the deletion.</returns>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] long id)
    {
        var result = await bookService.SoftDeleteAsync(id);

        return this.ToIActionResult(result);
    }

    /// <summary>
    /// Deletes multiple books in bulk.
    /// </summary>
    /// <param name="ids">A list of book IDs.</param>
    /// <returns>A response indicating the success or failure of the bulk deletion.</returns>
    [HttpDelete("bulk")]
    public async Task<IActionResult> DeleteBulkAsync([FromBody] List<long> ids)
    {
        var result = await bookService.SoftDeleteBulkAsync(ids);

        return this.ToIActionResult(result);
    }
}
