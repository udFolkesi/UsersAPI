using Core.Entities;
using Core.Entities.Abstractions;


namespace DAL.Repositories.Abstractions
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity, string revokedBy);
        Task RestoreAsync(TEntity entity);
    }
}
