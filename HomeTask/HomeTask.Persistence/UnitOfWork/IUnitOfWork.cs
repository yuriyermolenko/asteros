using System;
using HomeTask.Domain.Aggregates.ClientAgg;
using HomeTask.Domain.Aggregates.OrderAgg;
using HomeTask.Persistence.Repositories;

namespace HomeTask.Persistence.UnitOfWork
{
    // simplified version
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        #region Repositories

        IRepository<Client> ClientRepository { get; }
        IRepository<Order> OrderRepository { get; }  

        #endregion
    }
}