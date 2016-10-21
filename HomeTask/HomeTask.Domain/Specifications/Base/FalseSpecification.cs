using System;
using System.Linq.Expressions;

namespace HomeTask.Domain.Specifications.Base
{
    public sealed class FalseSpecification<TEntity>
        : Specification<TEntity> where TEntity : class
    {
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            const bool result = false;

            Expression<Func<TEntity, bool>> falseExpression = t => result;
            return falseExpression;
        }
    }
}