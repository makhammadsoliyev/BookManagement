using BookManagement.BusinessLogic.Common;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Api.Extensions;

public static class ControllerBaseExtensions
{
    public static IActionResult ToIActionResult(this ControllerBase controller, Result result)
    {
        return result.IsSuccess ? controller.NoContent() : controller.ProblemResult(result);
    }

    public static IActionResult ToIActionResult<T>(this ControllerBase controller, Result<T> result)
    {
        return result.IsSuccess ? controller.Ok(result.Value) : controller.ProblemResult(result);
    }

    private static IActionResult ProblemResult(this ControllerBase controller, Result result)
    {
        var (type, statusCode) = GetErrorDetails(result.Error);

        var details = new ProblemDetails
        {
            Title = result.Error?.Code ?? "Internal Server Error",
            Detail = result.Error?.Description ?? "An unexpected error occurred",
            Status = statusCode,
            Type = type,
            Instance = controller.HttpContext.Request.Path,
            Extensions = result.Error is ValidationError validationError
                ? new Dictionary<string, object>() { { "errors", validationError.Errors } }
                : null
        };

        var problem = controller.Problem(statusCode: statusCode);

        problem.Value = details;

        return problem;
    }

    private static readonly Dictionary<ErrorType, (string Type, int StatusCode)> ErrorMappings =
        new()
        {
            {
                    ErrorType.Unauthorized,
                    ("https://datatracker.ietf.org/doc/html/rfc7235#section-3.1", StatusCodes.Status401Unauthorized)
            },
            {
                    ErrorType.Validation,
                    ("https://tools.ietf.org/html/rfc7231#section-6.5.1", StatusCodes.Status400BadRequest)
            },
            {
                    ErrorType.Conflict,
                    ("https://tools.ietf.org/html/rfc7231#section-6.5.8", StatusCodes.Status409Conflict)
            },
            {
                    ErrorType.NotFound,
                    ("https://tools.ietf.org/html/rfc7231#section-6.5.4", StatusCodes.Status404NotFound)
            },
            {
                    ErrorType.Failure,
                    ("https://tools.ietf.org/html/rfc7231#section-6.5.1", StatusCodes.Status400BadRequest)
            }
        };

    private static (string Type, int StatusCode) GetErrorDetails(Error error)
    {
        return error is not null && ErrorMappings.TryGetValue(error.Type, out var details)
            ? details
            : ("https://tools.ietf.org/html/rfc7231#section-6.6.1", StatusCodes.Status500InternalServerError);
    }
}

