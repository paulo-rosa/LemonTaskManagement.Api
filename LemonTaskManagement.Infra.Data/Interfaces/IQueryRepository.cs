using LemonTaskManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LemonTaskManagement.Infra.Data.Interfaces;

public interface IQueryRepository<TEntity> : IDisposable where TEntity : EntityBase
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity> GetByAsync(int id);

    Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);
}
