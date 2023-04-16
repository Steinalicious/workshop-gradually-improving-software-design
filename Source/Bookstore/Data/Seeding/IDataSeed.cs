namespace Bookstore.Data.Seeding;

public interface IDataSeed<TEntity>
{
    Task SeedAsync();
    Task<TEntity> EnsureEqualExists(TEntity entity);

    async Task<IEnumerable<TEntity>> EnsureEqualExists(IEnumerable<TEntity> entities) =>
        await Task.WhenAll(entities.Select(EnsureEqualExists));
}