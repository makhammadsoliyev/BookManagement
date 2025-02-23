namespace BookManagement.Domain.Common;

public abstract class SoftDeletableAuditableEntity : AuditableEntity, ISoftDeletable
{
    public DateTime? DeletedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
}
