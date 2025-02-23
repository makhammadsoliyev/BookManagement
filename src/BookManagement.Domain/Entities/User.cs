using BookManagement.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace BookManagement.Domain.Entities;

public class User : IdentityUser<long>, ISoftDeletable
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? DeletedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
}
