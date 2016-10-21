using System;

namespace HomeTask.Persistence.UnitOfWork
{
    public interface IObjectContextTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}