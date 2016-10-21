namespace HomeTask.Persistence.UnitOfWork
{
    public class UnitOfWorkTransaction : IUnitOfWorkTransaction
    {
        private readonly IObjectContextTransaction _objectContextTransaction;

        public UnitOfWorkTransaction(IObjectContextTransaction objectContextTransaction)
        {
            _objectContextTransaction = objectContextTransaction;
        }

        public void Commit()
        {
            _objectContextTransaction.Commit();
        }

        public void Dispose()
        {
            _objectContextTransaction.Dispose();
        }

        public void Rollback()
        {
            _objectContextTransaction.Rollback();
        }
    }
}