using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using HomeTask.Domain.Aggregates.Base;
using HomeTask.Domain.Specifications.Base;

namespace HomeTask.Persistence.Repositories
{
    // simplified version
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbContext _dbContext;
        private readonly IObjectSet<TEntity> _objectSet;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _objectSet = dbContext.GetObjectContext().CreateObjectSet<TEntity>();
        }

        public IEnumerable<TEntity> Find(params Specification<TEntity>[] specifications)
        {
            var query = AsQueryable();

            query = specifications.Aggregate(
                query, (current, spec) =>
                current.Where(spec.SatisfiedBy()));

            return query.ToList();
        }

        public TEntity FirstOrDefault(params Specification<TEntity>[] specifications)
        {
            var query = AsQueryable();

            query = specifications.Aggregate(
                query, (current, spec) =>
                current.Where(spec.SatisfiedBy()));

            return query.FirstOrDefault();
        }

        public void Delete(TEntity entity)
        {
            _objectSet.DeleteObject(entity);
        }

        public void Insert(TEntity entity)
        {
            _objectSet.AddObject(entity);
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Rollback()
        {
            throw new System.NotImplementedException();
        }

        private IQueryable<TEntity> AsQueryable()
        {
            return _objectSet;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}