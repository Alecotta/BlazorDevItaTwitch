namespace BlazorDevIta.ERP.Infrastructure
{
    public interface IRepository<TEntity, Tkey>
        where TEntity : class, IEntity<Tkey>, new()
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity?> GetByIdAsync(Tkey id);

        Task Create(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(Tkey id);
    }
}
