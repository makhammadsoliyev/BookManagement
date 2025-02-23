using BookManagement.DataAccess.Infrastructure.Clock;
using BookManagement.DataAccess.Options;
using BookManagement.Domain.Entities;
using BookManagement.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BookManagement.DataAccess.Contexts;

public class ApplicationDbContextInitializer(
    ApplicationDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IDateTimeProvider dateTimeProvider,
    IOptions<AdminOptions> adminOptions) : IDisposable
{
    private readonly AdminOptions adminOptions = adminOptions.Value;

    public async Task AddDefaultDataAsync()
    {
        var accountRoles = Enum.GetNames(typeof(AccountRole));
        var roles = accountRoles.Select(accountRole => new Role
        {
            Name = accountRole,
        }).ToList();

        foreach (var role in roles)
        {
            bool isRoleExists = await roleManager.RoleExistsAsync(role.Name);
            if (!isRoleExists)
                await roleManager.CreateAsync(role);
        }

        var users = new List<User>
        {
            new()
            {
                FirstName = adminOptions.FirstName,
                LastName = adminOptions.LastName,
                Email = adminOptions.Email,
                UserName = adminOptions.Email,
                EmailConfirmed = true
            }
        };

        foreach (var user in users)
        {
            bool isUserExists = await userManager.FindByEmailAsync(user.Email) is not null;
            if (!isUserExists)
            {
                var result = await userManager.CreateAsync(user, adminOptions.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, nameof(AccountRole.Admin));
                    await userManager.AddToRoleAsync(user, nameof(AccountRole.User));
                }
            }
        }
    }

    public async Task SeedDataAsync()
    {
        if (await context.Books.AnyAsync())
            return;

        var books = new List<Book>
        {
            new()
            {
                Title = "Clean Code",
                AuthorName = "Robert C. Martin",
                PublicationYear = 2008
            },
            new()
            {
                Title = "The Pragmatic Programmer",
                AuthorName = "Andrew Hunt & David Thomas",
                PublicationYear = 1999
            },
            new()
            {
                Title = "Design Patterns",
                AuthorName = "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides",
                PublicationYear = 1994
            },
            new()
            {
                Title = "Refactoring",
                AuthorName = "Martin Fowler",
                PublicationYear = 1999
            },
            new()
            {
                Title = "Code Complete",
                AuthorName = "Steve McConnell",
                PublicationYear = 2004
            },
            new()
            {
                Title = "Introduction to the Theory of Computation",
                AuthorName = "Michael Sipser",
                PublicationYear = 1996
            },
            new()
            {
                Title = "Algorithms",
                AuthorName = "Robert Sedgewick & Kevin Wayne",
                PublicationYear = 2011
            },
            new()
            {
                Title = "Computer Networking: A Top-Down Approach",
                AuthorName = "James F. Kurose & Keith W. Ross",
                PublicationYear = 2000
            },
            new()
            {
                Title = "The Mythical Man-Month",
                AuthorName = "Frederick P. Brooks Jr.",
                PublicationYear = 1975
            },
            new()
            {
                Title = "You Don't Know JS",
                AuthorName = "Kyle Simpson",
                PublicationYear = 2015
            },
            new()
            {
                Title = "C# in Depth",
                AuthorName = "Jon Skeet",
                PublicationYear = 2019
            },
            new()
            {
                Title = "ASP.NET Core in Action",
                AuthorName = "Andrew Lock",
                PublicationYear = 2021
            },
            new()
            {
                Title = "Effective Java",
                AuthorName = "Joshua Bloch",
                PublicationYear = 2008
            },
            new()
            {
                Title = "Domain-Driven Design",
                AuthorName = "Eric Evans",
                PublicationYear = 2003
            },
            new()
            {
                Title = "Head First Design Patterns",
                AuthorName = "Eric Freeman & Elisabeth Robson",
                PublicationYear = 2004
            },
            new()
            {
                Title = "Eloquent JavaScript",
                AuthorName = "Marijn Haverbeke",
                PublicationYear = 2018
            },
            new()
            {
                Title = "Artificial Intelligence: A Modern Approach",
                AuthorName = "Stuart Russell & Peter Norvig",
                PublicationYear = 1995
            },
            new()
            {
                Title = "Deep Learning",
                AuthorName = "Ian Goodfellow, Yoshua Bengio, Aaron Courville",
                PublicationYear = 2016
            },
            new()
            {
                Title = "Patterns of Enterprise Application Architecture",
                AuthorName = "Martin Fowler",
                PublicationYear = 2002
            },
            new()
            {
                Title = "Grokking Algorithms",
                AuthorName = "Aditya Y. Bhargava",
                PublicationYear = 2016
            },
            new()
            {
                Title = "The Art of Computer Programming",
                AuthorName = "Donald Knuth",
                PublicationYear = 1968
            },
            new()
            {
                Title = "Programming Pearls",
                AuthorName = "Jon Bentley",
                PublicationYear = 1986
            },
            new()
            {
                Title = "Soft Skills: The Software Developer's Life Manual",
                AuthorName = "John Sonmez",
                PublicationYear = 2014
            },
            new()
            {
                Title = "Don't Make Me Think",
                AuthorName = "Steve Krug",
                PublicationYear = 2000
            }
        };

        books.ForEach(book => book.CreatedOnUtc = dateTimeProvider.UtcNow);

        await context.Books.AddRangeAsync(books);
        await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
