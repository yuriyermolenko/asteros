using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using HomeTask.Domain.Aggregates.Base;
using HomeTask.Domain.Specifications.Base;
using HomeTask.Persistence.UnitOfWork;

namespace HomeTask.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly IObjectSet<TEntity> _objectSet;

        public Repository(IObjectSetFactory objectSetFactory)
        {
            _objectSet = objectSetFactory.CreateObjectSet<TEntity>();
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

        private IQueryable<TEntity> AsQueryable()
        {
            return _objectSet;
        }
    }
}