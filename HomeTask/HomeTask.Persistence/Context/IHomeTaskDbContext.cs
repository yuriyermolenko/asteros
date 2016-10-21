using System;
using System.Data.Entity;

namespace HomeTask.Persistence.Context
{
    public interface IHomeTaskDbContext : IDisposable
    {
        int SaveChanges();
        DbSet<T> Set<T>() where T : class;
    }
}