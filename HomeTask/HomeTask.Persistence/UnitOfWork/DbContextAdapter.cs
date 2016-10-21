using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace HomeTask.Persistence.UnitOfWork
{
    public class DbContextAdapter : IObjectSetFactory, IObjectContext
    {
        private readonly ObjectContext _context;
        private readonly Database _db;

        public DbContextAdapter(DbContext context)
        {
            _context = context.GetObjectContext();
            _db = context.Database;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IObjectContextTransaction BeginTransaction()
        {
            return new ObjectContextTransaction(_db.BeginTransaction());
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IObjectSet<T> CreateObjectSet<T>() where T : class
        {
            return _context.CreateObjectSet<T>();
        }

        public void ChangeObjectState(object entity, EntityState state)
        {
            _context.ObjectStateManager.ChangeObjectState(entity, state);
        }
    }
}