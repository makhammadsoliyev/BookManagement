using BookManagement.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task AddDefaultDataAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var initializer = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContextInitializer>();

        await initializer.AddDefaultDataAsync();
    }

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        var pendingMigrations = context.Database.GetPendingMigrations().ToList();

        if (pendingMigrations.Count > 0)
            context.Database.Migrate();
    }

    public static async Task SeedDataAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var initializer = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContextInitializer>();

        await initializer.SeedDataAsync();
    }
}
