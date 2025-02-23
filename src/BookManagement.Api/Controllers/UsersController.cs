using BookManagement.Api.Extensions;
using BookManagement.BusinessLogic.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Api.Controllers;

/// <summary>
/// Controller responsible for managing users.
/// </summary>
[ApiController]
[Route("api/users")]
[Authorize(Roles = "Admin")]
public class UsersController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>The requested user details.</returns>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] long id)
    {
        var result = await userService.GetByIdAsync(id);

        return this.ToIActionResult(result);
    }

    /// <summary>
    /// Retrieves a list of all users.
    /// </summary>
    /// <returns>A list of all registered users.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await userService.GetAllAsync();

        return this.ToIActionResult(result);
    }
}
