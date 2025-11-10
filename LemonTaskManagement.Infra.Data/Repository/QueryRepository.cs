using LemonTaskManagement.Domain.Entities;
using LemonTaskManagement.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LemonTaskManagement.Infra.Data.Repository;

public abstract class QueryRepository<TEntity>(DbContext context) : DisposableBase(), IQueryRepository<TEntity>, IDisposable where TEntity : EntityBase
{
    protected readonly DbContext Db = context;

    protected readonly DbSet<TEntity> DbEntity = context.Set<TEntity>();

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await DbEntity.ToListAsync();
    }

    public virtual async Task<TEntity> GetByAsync(int id)
    {
        return await DbEntity.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbEntity.AsNoTracking().Where(predicate).ToListAsync();
    }

    protected override void PreDispose()
    {
        Db.Dispose();
    }
}
