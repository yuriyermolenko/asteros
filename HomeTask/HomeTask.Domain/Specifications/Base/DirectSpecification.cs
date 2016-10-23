using System;
using System.Linq.Expressions;

namespace HomeTask.Domain.Specifications.Base
{
    public class DirectSpecification<TEntity>
        : Specification<TEntity>
        where TEntity : class
    {
        private readonly Expression<Func<TEntity, bool>> _matchingCriteria;

        public DirectSpecification(Expression<Func<TEntity, bool>> matchingCriteria)
        {
            if (matchingCriteria == null)
            {
                throw new ArgumentNullException("matchingCriteria");
            }

            _matchingCriteria = matchingCriteria;
        }

        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            return _matchingCriteria;
        }
    }
}