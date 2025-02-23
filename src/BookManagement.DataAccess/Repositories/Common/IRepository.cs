using BookManagement.Domain.Common;
using System.Linq.Expressions;

namespace BookManagement.DataAccess.Repositories.Common;

public interface IRepository<TEntity> where TEntity : AuditableEntity
{
    Task AddAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression);

    Task UpdateAsync(TEntity entity);

    Task RemoveAsync(TEntity entity);

    Task RemoveRangeAsync(IEnumerable<TEntity> entities);

    IQueryable<TEntity> GetAll();
}
