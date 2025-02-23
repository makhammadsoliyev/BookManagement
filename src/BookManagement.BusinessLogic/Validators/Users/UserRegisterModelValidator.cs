using BookManagement.BusinessLogic.Models.Users;
using FluentValidation;

namespace BookManagement.BusinessLogic.Validators.Users;

public class UserRegisterModelValidator : AbstractValidator<UserRegisterModel>
{
    public UserRegisterModelValidator()
    {
        RuleFor(user => user.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(user => user.LastName).NotEmpty().MaximumLength(100);

        RuleFor(user => user.Email)
            .NotEmpty()
            .Matches("^[\\w\\.-]+@[a-zA-Z\\d\\.-]+\\.[a-zA-Z]{2,}$")
            .WithMessage("Invalid email format.");

        RuleFor(user => user.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

        RuleFor(user => user.ConfirmPassword).NotEmpty().Equal(user => user.Password);
    }
}