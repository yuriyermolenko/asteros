using System;
using System.Collections.Generic;
using HomeTask.Domain.Aggregates.Base;
using HomeTask.Domain.Specifications.Base;

namespace HomeTask.Persistence.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class, IEntity
    {
        IEnumerable<TEntity> Find(params Specification<TEntity>[] specifications);
        TEntity FirstOrDefault(params Specification<TEntity>[] specifications);
        void Delete(TEntity entity);
        void Insert(TEntity entity);
    }
}