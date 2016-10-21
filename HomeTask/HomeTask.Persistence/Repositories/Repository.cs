using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HomeTask.Domain.Aggregates.Base;
using HomeTask.Domain.Specifications.Base;
using HomeTask.Persistence.Context;

namespace HomeTask.Persistence.Repositories
{
    // simplified version
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly IHomeTaskDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(IHomeTaskDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
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
            _dbSet.Remove(entity);
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        private IQueryable<TEntity> AsQueryable()
        {
            return _dbSet;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}