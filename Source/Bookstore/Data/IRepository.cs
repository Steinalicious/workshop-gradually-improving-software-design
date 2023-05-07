namespace Bookstore.Data;

public interface IRepository<TEntity>
{
    IQueryable<TEntity> All { get; }
    void Add(TEntity entity);
    void Remove(TEntity entity);
}