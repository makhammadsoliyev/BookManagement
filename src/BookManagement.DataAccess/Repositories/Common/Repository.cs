using BookManagement.DataAccess.Contexts;
using BookManagement.DataAccess.Infrastructure.Clock;
using BookManagement.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookManagement.DataAccess.Repositories.Common;

public abstract class Repository<TEntity>(
    ApplicationDbContext context,
    IDateTimeProvider dateTimeProvider)
    : IRepository<TEntity> where TEntity : AuditableEntity
{
    protected readonly DbSet<TEntity> dbSet = context.Set<TEntity>();

    public async Task AddAsync(TEntity entity)
    {
        entity.CreatedOnUtc = dateTimeProvider.UtcNow;
        await dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            entity.CreatedOnUtc = dateTimeProvider.UtcNow;

        await dbSet.AddRangeAsync(entities);
    }

    public async Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> expression)
    {
        return await dbSet.FirstOrDefaultAsync(expression);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        entity.UpdatedOnUtc = dateTimeProvider.UtcNow;
        dbSet.Update(entity);

        await Task.CompletedTask;
    }

    public async Task RemoveAsync(TEntity entity)
    {
        switch (entity)
        {
            case ISoftDeletable softDeletable:
                softDeletable.IsDeleted = true;
                softDeletable.DeletedOnUtc = dateTimeProvider.UtcNow;
                dbSet.Update(entity);
                break;
            default:
                dbSet.Remove(entity);
                break;
        }

        await Task.CompletedTask;
    }

    public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
    {
        var softDeletableEntities = new List<TEntity>();
        var hardDeletableEntities = new List<TEntity>();

        foreach (var entity in entities)
            if (entity is ISoftDeletable softDeletable)
            {
                softDeletable.DeletedOnUtc = dateTimeProvider.UtcNow;
                softDeletable.IsDeleted = true;
                softDeletableEntities.Add(entity);
            }
            else
            {
                hardDeletableEntities.Add(entity);
            }

        if (softDeletableEntities.Count > 0)
            dbSet.UpdateRange(softDeletableEntities);

        if (hardDeletableEntities.Count > 0)
            dbSet.RemoveRange(hardDeletableEntities);

        await Task.CompletedTask;
    }

    public IQueryable<TEntity> GetAll()
    {
        return dbSet;
    }
}
