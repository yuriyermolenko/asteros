using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeTask.Domain.Aggregates.Base;
using HomeTask.Domain.Specifications.Base;

namespace HomeTask.Persistence.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class, IEntity
    {
        List<TEntity> Find(params Specification<TEntity>[] specifications);
        Task<List<TEntity>> FindAsync(params Specification<TEntity>[] specifications);
        TEntity FirstOrDefault(params Specification<TEntity>[] specifications);
        Task<TEntity> FirstOrDefaultAsync(params Specification<TEntity>[] specifications);
        void Delete(TEntity entity);
        void Insert(TEntity entity);
    }
}