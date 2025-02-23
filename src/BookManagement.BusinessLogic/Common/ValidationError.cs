using FluentValidation.Results;

namespace BookManagement.BusinessLogic.Common;

public record ValidationError(Error[] Errors)
    : Error("Validation.General", "One or more validation errors occured", ErrorType.Validation)
{
    public static ValidationError FromValidationFailures(List<ValidationFailure> validationFailures) =>
        new(validationFailures.Select(f => new Error(f.ErrorCode, f.ErrorMessage, ErrorType.Validation)).ToArray());
}
