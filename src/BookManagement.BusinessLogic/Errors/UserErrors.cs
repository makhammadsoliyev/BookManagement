using BookManagement.BusinessLogic.Common;

namespace BookManagement.BusinessLogic.Errors;

public static class UserErrors
{
    public static Error NotFound(long id) => Error.NotFound(
        "Users.NotFound",
        $"The user is not found with this id: {id}");

    public static Error Unauthorized() => Error.Unauthorized(
        "Users.Unauthorized",
        "You are not authorized to perfrom this action");

    public static Error NotFoundByEmail(string email) => Error.NotFound(
        "Users.NotFoundByEmail",
        $"The user is not found with this email: {email}");

    public static Error EmailNotUnique() => Error.Conflict(
        "Users.EmailNotUnique",
        "The provided email is not unique");

    public static Error InvalidCredentials() => Error.Unauthorized(
        "Users.InvalidCredentials",
        "The email or password is incorrect.");

    public static Error RegistrationFailed() => Error.Failure(
        "Users.RegistrationFailed",
        "User registration failed due to an unknown error");
}
