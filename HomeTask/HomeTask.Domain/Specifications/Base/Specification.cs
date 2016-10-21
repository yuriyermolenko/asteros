﻿using System;
using System.Linq.Expressions;

namespace HomeTask.Domain.Specifications.Base
{
    public abstract class Specification<TEntity> : ISpecification<TEntity>
         where TEntity : class
    {
        public abstract Expression<Func<TEntity, bool>> SatisfiedBy();

        public static Specification<TEntity> operator &(Specification<TEntity> leftSideSpecification, Specification<TEntity> rightSideSpecification)
        {
            return new AndSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        public static Specification<TEntity> operator |(Specification<TEntity> leftSideSpecification, Specification<TEntity> rightSideSpecification)
        {
            return new OrSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        public static Specification<TEntity> operator !(Specification<TEntity> specification)
        {
            return new NotSpecification<TEntity>(specification);
        }

        public static bool operator false(Specification<TEntity> specification)
        {
            return false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "specification")]
        public static bool operator true(Specification<TEntity> specification)
        {
            return true;
        }
    }
}