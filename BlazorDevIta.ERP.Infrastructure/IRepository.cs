namespace BlazorDevIta.ERP.Infrastructure
{
    public interface IRepository<TEntity, Tkey>
        where TEntity : class, IEntity<Tkey>, new()
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity?> GetByIdAsync(Tkey id);

        Task CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(Tkey id);
    }
}
