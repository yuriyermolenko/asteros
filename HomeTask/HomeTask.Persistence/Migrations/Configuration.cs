using HomeTask.Domain.Aggregates.Base;
using HomeTask.Domain.Aggregates.ClientAgg;
using HomeTask.Domain.Specifications;
using HomeTask.Persistence.Context;
using HomeTask.Persistence.Repositories;

namespace HomeTask.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<HomeTaskDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HomeTaskDbContext context)
        {
            AddClient("������ ��������", true, context);
            AddClient("�����-���������", false, context);
            AddClient("������", true, context);
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
            return new Repository<T>(context);
        }
    }
}
