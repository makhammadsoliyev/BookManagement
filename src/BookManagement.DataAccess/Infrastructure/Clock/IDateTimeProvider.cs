namespace BookManagement.DataAccess.Infrastructure.Clock;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
