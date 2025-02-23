using BookManagement.Domain.Common;

namespace BookManagement.Domain.Entities;

public class Book : SoftDeletableAuditableEntity
{
    public string Title { get; set; }

    public int PublicationYear { get; set; }

    public string AuthorName { get; set; }

    public int Views { get; set; }
}
