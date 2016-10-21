using System.Data.Entity;

namespace HomeTask.Persistence.UnitOfWork
{
    public class ObjectContextTransaction : IObjectContextTransaction
    {
        private readonly DbContextTransaction _transaction;

        public ObjectContextTransaction(DbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }
    }
}