using HomeTask.Persistence.UnitOfWork;

namespace MyDeals.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();

        IUnitOfWorkTransaction BeginTransaction();
    }
}