using System;

namespace HomeTask.Persistence.UnitOfWork
{
    public interface IObjectContext : IDisposable
    {
        void SaveChanges();
        IObjectContextTransaction BeginTransaction();
    }
}