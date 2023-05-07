using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data.Implementation;

public class DbSetRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly IQueryable<TEntity> _baseQuery;

    public DbSetRepository(DbSet<TEntity> dbSet, IQueryable<TEntity> baseQuery) =>
        (_dbSet, _baseQuery) = (dbSet, baseQuery);

    public IQueryable<TEntity> All => _baseQuery;

    public void Add(TEntity entity) => _dbSet.Add(entity);

    public void Remove(TEntity entity) => _dbSet.Remove(entity);
}
