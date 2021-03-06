﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace HomeTask.Domain.Specifications.Base
{
    public sealed class NotSpecification<TEntity>
        : Specification<TEntity> where TEntity : class
    {
        private readonly Expression<Func<TEntity, bool>> _originalCriteria;

        public NotSpecification(ISpecification<TEntity> originalSpecification)
        {
            if (originalSpecification == null)
            {
                throw new ArgumentNullException(nameof(originalSpecification));
            }

            _originalCriteria = originalSpecification.SatisfiedBy();
        }

        public NotSpecification(Expression<Func<TEntity, bool>> originalSpecification)
        {
            if (originalSpecification == null)
            {
                throw new ArgumentNullException(nameof(originalSpecification));
            }

            _originalCriteria = originalSpecification;
        }

        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            return Expression.Lambda<Func<TEntity, bool>>(Expression.Not(_originalCriteria.Body),
                                                         _originalCriteria.Parameters.Single());
        }
    }
}