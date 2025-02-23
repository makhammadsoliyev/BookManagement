using AutoMapper;
using BookManagement.BusinessLogic.Common;
using BookManagement.BusinessLogic.Errors;
using BookManagement.BusinessLogic.Extensions;
using BookManagement.BusinessLogic.Models.Users;
using BookManagement.BusinessLogic.Providers;
using BookManagement.Domain.Entities;
using BookManagement.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BookManagement.BusinessLogic.Services.Auth;

public class AuthService(
    IMapper mapper,
    ITokenProvider tokenProvider,
    UserManager<User> userManager,
    IValidator<UserLoginModel> loginValidator,
    IValidator<UserRegisterModel> registerValidator,
    SignInManager<User> signInManager) : IAuthService
{
    public async Task<Result<string>> LoginAsync(UserLoginModel userLoginModel)
    {
        var validationResult = await loginValidator.ValidateModelsAsync(userLoginModel);
        if (validationResult.IsFailure)
            return Result.Failure<string>(validationResult.Error);

        var user = await userManager.FindByEmailAsync(userLoginModel.Email);
        if (user is null)
            return Result.Failure<string>(UserErrors.NotFoundByEmail(userLoginModel.Email));

        var result = await signInManager.PasswordSignInAsync(user, userLoginModel.Password, false, false);
        if (!result.Succeeded)
            return Result.Failure<string>(UserErrors.InvalidCredentials());

        var token = await tokenProvider.CreateAsync(user);

        return token;
    }

    public async Task<Result<long>> RegisterAsync(UserRegisterModel userRegisterModel)
    {
        var validationResult = await registerValidator.ValidateModelsAsync(userRegisterModel);
        if (validationResult.IsFailure)
            return Result.Failure<long>(validationResult.Error);

        var existUser = await userManager.FindByEmailAsync(userRegisterModel.Email);
        if (existUser is not null)
            return Result.Failure<long>(UserErrors.EmailNotUnique());

        var user = mapper.Map<User>(userRegisterModel);
        user.UserName = userRegisterModel.Email;

        var result = await userManager.CreateAsync(user, userRegisterModel.Password);
        if (!result.Succeeded)
            return Result.Failure<long>(UserErrors.RegistrationFailed());

        await userManager.AddToRoleAsync(user, nameof(AccountRole.User));

        return user.Id;
    }
}
