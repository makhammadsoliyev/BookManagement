using BookManagement.BusinessLogic.Common;
using BookManagement.BusinessLogic.Models.Users;

namespace BookManagement.BusinessLogic.Services.Users;

public interface IUserService
{
    Task<Result<UserResultModel>> GetByIdAsync(long id);

    Task<Result<List<UserResultModel>>> GetAllAsync();
}
