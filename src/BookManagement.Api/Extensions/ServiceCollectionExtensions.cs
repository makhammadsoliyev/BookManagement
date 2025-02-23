using BookManagement.Api.Handlers;
using BookManagement.BusinessLogic.Mappers;
using BookManagement.BusinessLogic.Options;
using BookManagement.BusinessLogic.Providers;
using BookManagement.BusinessLogic.Services.Auth;
using BookManagement.BusinessLogic.Services.Books;
using BookManagement.BusinessLogic.Services.Users;
using BookManagement.BusinessLogic.Validators.Books;
using BookManagement.DataAccess.Contexts;
using BookManagement.DataAccess.Infrastructure.Clock;
using BookManagement.DataAccess.Options;
using BookManagement.DataAccess.Repositories.Books;
using BookManagement.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace BookManagement.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSwaggerGenWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Book Management API",
                Version = "v1"
            });

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Auhthorization",
                Description = "Enter your JWT token in this field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            };

            options.AddSecurityDefinition("Bearer", securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                    },
                    []
                }
            };

            options.AddSecurityRequirement(securityRequirement);

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }

    public static void AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Database")).UseSnakeCaseNamingConvention());

        services.AddScoped<ApplicationDbContextInitializer>();
        services.ConfigureOptions<AdminOptionsSetup>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();
    }

    public static void AddProviders(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<ITokenProvider, TokenProvider>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(BookBaseValidator<>).Assembly);
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBookService, BookService>();
    }

    public static void AddIdentityServer(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    }

    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var jwtOptions = new JwtOptions();
            configuration
            .GetRequiredSection(nameof(JwtOptions))
            .Bind(jwtOptions);
            options.Audience = jwtOptions.Audience;
            options.Authority = jwtOptions.Issuer;

            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
            };
        });

        services.ConfigureOptions<JwtOptionsSetup>();
        services.AddSingleton<JwtSecurityTokenHandler>();
    }

    public static void AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
}
