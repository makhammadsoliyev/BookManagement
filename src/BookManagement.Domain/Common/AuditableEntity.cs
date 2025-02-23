namespace BookManagement.Domain.Common;

public abstract class AuditableEntity
{
    public long Id { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }
}
