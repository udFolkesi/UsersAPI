using Core.Entities;
using Core.Entities.Abstractions;


namespace DAL.Repositories.Abstractions
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity, string revokedBy);
    }
}
