using BookManagement.BusinessLogic.Models.Users;
using FluentValidation;

namespace BookManagement.BusinessLogic.Validators.Users;

public class UserLoginModelValidator : AbstractValidator<UserLoginModel>
{
    public UserLoginModelValidator()
    {
        RuleFor(user => user.Email).NotEmpty().EmailAddress();
        RuleFor(user => user.Password).NotEmpty();
    }
}