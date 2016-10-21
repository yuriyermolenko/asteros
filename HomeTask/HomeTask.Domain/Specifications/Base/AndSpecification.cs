using System;
using System.Linq.Expressions;

namespace HomeTask.Domain.Specifications.Base
{
    public sealed class AndSpecification<T>
       : CompositeSpecification<T>
       where T : class
    {
        private readonly ISpecification<T> _rightSideSpecification;
        private readonly ISpecification<T> _leftSideSpecification;

        public AndSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            if (leftSide == null)
            {
                throw new ArgumentNullException(nameof(leftSide));
            }

            if (rightSide == null)
            {
                throw new ArgumentNullException(nameof(rightSide));
            }

            _leftSideSpecification = leftSide;
            _rightSideSpecification = rightSide;
        }

        public override ISpecification<T> LeftSideSpecification => _leftSideSpecification;
        public override ISpecification<T> RightSideSpecification => _rightSideSpecification;

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            var left = _leftSideSpecification.SatisfiedBy();
            var right = _rightSideSpecification.SatisfiedBy();

            return left.And(right);
        }
    }
}