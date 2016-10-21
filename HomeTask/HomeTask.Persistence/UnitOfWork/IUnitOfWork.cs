namespace HomeTask.Persistence.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();

        IUnitOfWorkTransaction BeginTransaction();
    }
}