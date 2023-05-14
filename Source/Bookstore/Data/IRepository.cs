using Bookstore.Common;

namespace Bookstore.Data;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> All { get; }
    void Add(TEntity entity);
    void Remove(TEntity entity);

    Task<List<TEntity>> QueryAsync(ISpecification<TEntity> specification);
    Task<Option<TEntity>> SingleOrNoneAsync(ISpecification<TEntity> specification);
}