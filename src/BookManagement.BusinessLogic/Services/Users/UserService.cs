using AutoMapper;
using BookManagement.BusinessLogic.Common;
using BookManagement.BusinessLogic.Errors;
using BookManagement.BusinessLogic.Models.Users;
using BookManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.BusinessLogic.Services.Users;

public class UserService(IMapper mapper, UserManager<User> userManager) : IUserService
{
    public async Task<Result<UserResultModel>> GetByIdAsync(long id)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(user => user.Id == id);
        if (user is null)
            return Result.Failure<UserResultModel>(UserErrors.NotFound(id));

        return mapper.Map<UserResultModel>(user);
    }

    public async Task<Result<List<UserResultModel>>> GetAllAsync()
    {
        var users = await userManager.Users.ToListAsync();

        return mapper.Map<List<UserResultModel>>(users);
    }
}