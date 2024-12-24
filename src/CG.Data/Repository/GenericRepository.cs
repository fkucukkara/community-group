using CG.Core.Domain;
using CG.Data.DBContext;
using CG.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CG.Data.Repository;

public class GenericRepository<TEntity>(CGDBContext context) : IRepository<TEntity> where TEntity : BaseEntity
{
    public IQueryable<TEntity> GetQueryable()
    {
        return context.Set<TEntity>().AsQueryable().AsNoTracking();
    }
    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);
    }
    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        entity.CreatedOn = DateTime.UtcNow;
        await context.Set<TEntity>().AddAsync(entity).ConfigureAwait(false);
        await context.SaveChangesAsync().ConfigureAwait(false);
        return entity;
    }
    public async Task<IEnumerable<TEntity>> CreateRangeAsync(IEnumerable<TEntity> entities)
    {
        entities.ToList().ForEach(x => x.CreatedOn = DateTime.UtcNow);
        await context.Set<TEntity>().AddRangeAsync(entities).ConfigureAwait(false);
        await context.SaveChangesAsync().ConfigureAwait(false);
        return entities;
    }
    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        entity.ModifiedOn = DateTime.UtcNow;
        context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync().ConfigureAwait(false);
        return entity;
    }
    public async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        entities.ToList().ForEach(a => a.ModifiedOn = DateTime.UtcNow);
        context.Set<TEntity>().UpdateRange(entities);
        await context.SaveChangesAsync().ConfigureAwait(false);
        return entities;
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await context.Set<TEntity>().FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        entities.ToList().ForEach(a => a.IsDeleted = true);
        context.Set<TEntity>().UpdateRange(entities);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }
}
