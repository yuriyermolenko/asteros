using HomeTask.Domain.Aggregates.Base;
using HomeTask.Domain.Aggregates.ClientAgg;
using HomeTask.Domain.Specifications;
using HomeTask.Persistence.Repositories;
using HomeTask.Persistence.UnitOfWork;

namespace HomeTask.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<HomeTask.Persistence.HomeTaskDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HomeTaskDbContext context)
        {
            AddClient("Нижний Новгород", true, context);
            AddClient("Санкт-Петербург", false, context);
            AddClient("Москва", true, context);
        }

        private static int AddClient(string address, bool vip, HomeTaskDbContext context)
        {
            var clientRepository = CreateRepository<Client>(context);

            var client = clientRepository.FirstOrDefault(ClientSpecifications.ByAddress(address));

            if (client == null)
            {
                client = new Client {Address = address, VIP = vip};
                clientRepository.Insert(client);

                context.SaveChanges();
            }

            return client.Id;
        }

        private static IRepository<T> CreateRepository<T>(HomeTaskDbContext context) where T : class, IEntity
        {
            var dbAdapter = new DbContextAdapter(context);
            return new Repository<T>(dbAdapter);
        }
    }
}
