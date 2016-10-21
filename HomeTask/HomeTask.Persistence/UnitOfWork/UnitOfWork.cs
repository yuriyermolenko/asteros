using HomeTask.Domain.Aggregates.ClientAgg;
using HomeTask.Domain.Aggregates.OrderAgg;
using HomeTask.Persistence.Context;
using HomeTask.Persistence.Repositories;

namespace HomeTask.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IHomeTaskDbContext _dbContext;

        private IRepository<Client> _clientRepository;
        public IRepository<Client> ClientRepository => _clientRepository ?? (_clientRepository = new Repository<Client>(_dbContext));

        private IRepository<Order> _orderRepository;
        public IRepository<Order> OrderRepository => _orderRepository ?? (_orderRepository = new Repository<Order>(_dbContext));

        public UnitOfWork(IHomeTaskDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}