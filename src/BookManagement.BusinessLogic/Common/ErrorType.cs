namespace BookManagement.BusinessLogic.Common;

public enum ErrorType : byte
{
    Failure, // Status400BadRequest
    Validation, // Status400BadRequest
    NotFound, // Status404NotFound
    Conflict, // Status409Conflict
    Unauthorized // Status401Unauthorized
}