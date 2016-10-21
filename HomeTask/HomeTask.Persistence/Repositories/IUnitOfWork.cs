namespace HomeTask.Persistence.Repositories
{
    // simplified version
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
    }
}