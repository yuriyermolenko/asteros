﻿namespace HomeTask.Domain.Specifications.Base
{
    public abstract class CompositeSpecification<TEntity>
            : Specification<TEntity> where TEntity : class
    {
        public abstract ISpecification<TEntity> LeftSideSpecification { get; }
        public abstract ISpecification<TEntity> RightSideSpecification { get; }
    }
}