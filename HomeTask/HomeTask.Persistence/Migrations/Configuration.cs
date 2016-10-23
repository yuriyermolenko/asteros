using HomeTask.Domain.Aggregates.Base;
using HomeTask.Domain.Aggregates.ClientAgg;
using HomeTask.Domain.Aggregates.OrderAgg;
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
            var id = AddClient("������ ��������", true, context);
            AddOrder(id, "������� ���������� 1", context);
            AddOrder(id, "������� ������", context);
            AddOrder(id, "�������� ��������", context);
            AddClient("�����-���������", false, context);
            id = AddClient("������", true, context);
            AddOrder(id, "������", context);
            
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

        private static int AddOrder(int clientId, string description, HomeTaskDbContext context)
        {
            var clientRepository = CreateRepository<Client>(context);
            var orderRepository = CreateRepository<Order>(context);

            var client = clientRepository.FirstOrDefault(ClientSpecifications.ById(clientId));
            if (client != null)
            {
                var order = orderRepository.FirstOrDefault(OrderSpecifications.ByDescription(description));
                if (order == null)
                {
                    order = new Order() {Description = description, ClientId = clientId};
                    orderRepository.Insert(order);
                    context.SaveChanges();
                    return order.Id;
                }
                return order.Id;
            }

            return -1;
        }

        private static IRepository<T> CreateRepository<T>(HomeTaskDbContext context) where T : class, IEntity
        {
            return new Repository<T>(context);
        }
    }
}
