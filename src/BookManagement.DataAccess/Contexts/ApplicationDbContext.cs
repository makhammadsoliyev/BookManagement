using BookManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.DataAccess.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<User,
            Role,
            long,
            IdentityUserClaim<long>,
            UserRole,
            IdentityUserLogin<long>,
            IdentityRoleClaim<long>,
            IdentityUserToken<long>>(options),
    IUnitOfWork
{
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Ignore<IdentityUserLogin<long>>();
        builder.Ignore<IdentityUserToken<long>>();

        builder.Entity<User>()
            .Ignore(user => user.AccessFailedCount)
            .Ignore(user => user.LockoutEnabled)
            .Ignore(user => user.LockoutEnd)
            .Ignore(user => user.TwoFactorEnabled)
            .Ignore(user => user.PhoneNumber)
            .Ignore(user => user.PhoneNumberConfirmed);

        builder.Entity<User>().ToTable("users");
        builder.Entity<Role>().ToTable("roles");
        builder.Entity<UserRole>().ToTable("user_roles");
        builder.Entity<IdentityUserClaim<long>>().ToTable("user_claims");
        builder.Entity<IdentityRoleClaim<long>>().ToTable("role_claims");

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
