using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace HomeTask.Persistence.UnitOfWork
{
    public interface IObjectSetFactory : IDisposable
    {
        IObjectSet<T> CreateObjectSet<T>() where T : class;
        void ChangeObjectState(object entity, EntityState state);
    }
}