using System;

namespace HomeTask.Persistence.UnitOfWork
{
    public interface IUnitOfWorkTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}