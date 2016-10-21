using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace HomeTask.Persistence.Context
{
    public interface IHomeTaskDbContext : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbSet<T> Set<T>() where T : class;
    }
}