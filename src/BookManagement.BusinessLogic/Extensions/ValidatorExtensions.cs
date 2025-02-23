using BookManagement.BusinessLogic.Common;
using FluentValidation;

namespace BookManagement.BusinessLogic.Extensions;

public static class ValidatorExtensions
{
    public static async Task<Result> ValidateModelsAsync<TValidator, TModel>(
        this TValidator validator, params TModel[] models)
            where TValidator : IValidator<TModel>
            where TModel : class
    {
        var validationResults = await Task.WhenAll(models.Select(model => validator.ValidateAsync(model)));
        var validationFailures = validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToList();

        return validationFailures.Count > 0
            ? Result.Failure(ValidationError.FromValidationFailures(validationFailures))
            : Result.Success();
    }

}
