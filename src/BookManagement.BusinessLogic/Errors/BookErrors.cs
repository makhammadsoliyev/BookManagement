using BookManagement.BusinessLogic.Common;

namespace BookManagement.BusinessLogic.Errors;

public static class BookErrors
{
    public static Error DuplicateTitle() => Error.Conflict(
        "Books.DuplicateTitle",
        "The book with this title is already exist");

    public static Error DuplicateTitles(IEnumerable<string> titles) => Error.Conflict(
        "Books.DuplicateTitles",
        $"Books with these titles already exist: {string.Join(", ", titles)}");


    public static Error NotFound(long id) => Error.NotFound(
        "Books.NotFound",
        $"The book is not found with this id: {id}");

    public static Error NotFoundByTitle(string title) => Error.NotFound(
        "Books.NotFoundByTitle",
        $"The book is not found with this title: {title}");

    public static Error NotFoundAny() => Error.NotFound(
        "Books.NotFoundAny",
        "No books were found");
}
