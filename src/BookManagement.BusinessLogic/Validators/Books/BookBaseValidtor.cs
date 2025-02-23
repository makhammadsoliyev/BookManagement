using BookManagement.BusinessLogic.Models.Books;
using FluentValidation;

namespace BookManagement.BusinessLogic.Validators.Books;

public class BookBaseValidator<TModel> : AbstractValidator<TModel> where TModel : BookBaseModel
{
    public BookBaseValidator()
    {
        RuleFor(book => book.Title).NotEmpty().MaximumLength(255);
        RuleFor(book => book.AuthorName).NotEmpty().MaximumLength(255);
        RuleFor(book => book.PublicationYear).NotEmpty();
    }
}
