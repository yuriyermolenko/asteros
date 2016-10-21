namespace HomeTask.Persistence.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateScope();
    }
}