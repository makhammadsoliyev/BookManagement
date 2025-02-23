﻿namespace BookManagement.BusinessLogic.Common;

public record Error(string Code, string Description, ErrorType Type)
{
    public static Error None => new(string.Empty, string.Empty, ErrorType.Failure);

    public static Error NullValue => new("Error.NullValue", "Null value was provided", ErrorType.Failure);

    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    public static Error Unauthorized(string code, string description) =>
        new(code, description, ErrorType.Unauthorized);
}
