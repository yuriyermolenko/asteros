using System;

namespace HomeTask.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IObjectContext _objectContext;

        public UnitOfWork(IObjectContext objectContext)
        {
            _objectContext = objectContext;
        }

        public void Commit()
        {
            _objectContext.SaveChanges();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public IUnitOfWorkTransaction BeginTransaction()
        {
            var transaction = new UnitOfWorkTransaction(_objectContext.BeginTransaction());

            return transaction;
        }

        #region IDisposable Members

        public void Dispose()
        {
            _objectContext?.Dispose();

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}