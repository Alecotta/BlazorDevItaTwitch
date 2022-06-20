using Microsoft.EntityFrameworkCore;

namespace BlazorDevIta.ERP.Infrastructure.EF
{
    public class EFRepository<TEntity, Tkey>
        : IRepository<TEntity, Tkey>
        where TEntity : class, IEntity<Tkey>, new()
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _set;

        public EFRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _set = _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _set.AsNoTracking();
        }

        public async Task<TEntity?> GetByIdAsync(Tkey id)
        {
            var entity = await _set.FindAsync(id).AsTask();
            if (entity == null) return null;

            _dbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task CreateAsync(TEntity entity)
        {
            _set.Add(entity);
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(entity).State = EntityState.Detached;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _set.Update(entity);
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(entity).State = EntityState.Detached;
        }

        public Task DeleteAsync(Tkey id)
        {
            var entity = new TEntity()
            {
                Id = id
            };
            _set.Remove(entity);
            return _dbContext.SaveChangesAsync();
        }
    }
}