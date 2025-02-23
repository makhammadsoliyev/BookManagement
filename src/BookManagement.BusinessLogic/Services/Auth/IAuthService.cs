using BookManagement.BusinessLogic.Common;
using BookManagement.BusinessLogic.Models.Users;

namespace BookManagement.BusinessLogic.Services.Auth;

public interface IAuthService
{
    Task<Result<long>> RegisterAsync(UserRegisterModel userRegisterModel);

    Task<Result<string>> LoginAsync(UserLoginModel userLoginModel);
}