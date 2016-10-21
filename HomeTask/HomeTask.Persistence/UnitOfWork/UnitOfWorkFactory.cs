using HomeTask.Persistence.Context;

namespace HomeTask.Persistence.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork CreateScope()
        {
            return new UnitOfWork(new HomeTaskDbContext());
        }
    }
}