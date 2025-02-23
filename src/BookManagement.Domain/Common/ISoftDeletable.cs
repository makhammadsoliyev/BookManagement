namespace BookManagement.Domain.Common;

public interface ISoftDeletable
{
    DateTime? DeletedOnUtc { get; set; }

    bool IsDeleted { get; set; }
}