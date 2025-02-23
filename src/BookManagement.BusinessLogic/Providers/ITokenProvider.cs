using BookManagement.Domain.Entities;

namespace BookManagement.BusinessLogic.Providers;

public interface ITokenProvider
{
    Task<string> CreateAsync(User user);
}
