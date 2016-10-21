using System;
using System.Linq.Expressions;

namespace HomeTask.Domain.Specifications.Base
{
    public interface ISpecification<TEntity>
        where TEntity : class
    {
        Expression<Func<TEntity, bool>> SatisfiedBy();
    }
}