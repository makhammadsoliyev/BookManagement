using BookManagement.Api.Extensions;
using BookManagement.BusinessLogic.Models.Users;
using BookManagement.BusinessLogic.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Api.Controllers;

/// <summary>
/// Controller responsible for user authentication.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="model">The user registration model containing user details.</param>
    /// <returns>A response indicating the success or failure of the registration.</returns>
    /// <response code="200">Returns if the user was successfully registered.</response>
    /// <response code="400">Returns if the request data is invalid.</response>
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterModel model)
    {
        var result = await authService.RegisterAsync(model);

        return this.ToIActionResult(result);
    }

    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="model">The user login model containing credentials.</param>
    /// <returns>A response with authentication token or failure message.</returns>
    /// <response code="200">Returns if the user was successfully authenticated.</response>
    /// <response code="400">Returns if the login data is invalid.</response>
    /// <response code="401">Returns if authentication fails.</response>
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginModel model)
    {
        var result = await authService.LoginAsync(model);

        return this.ToIActionResult(result);
    }
}
