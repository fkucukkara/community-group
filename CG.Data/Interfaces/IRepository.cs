using CG.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CG.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetQueryable();
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> CreateRangeAsync(IEnumerable<TEntity> entities);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(int id);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    }
}
