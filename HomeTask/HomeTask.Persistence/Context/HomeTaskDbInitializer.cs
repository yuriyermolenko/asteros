﻿using System.Data.Entity;
using HomeTask.Domain.Aggregates.Base;
using HomeTask.Domain.Aggregates.ClientAgg;
using HomeTask.Domain.Aggregates.OrderAgg;
using HomeTask.Domain.Specifications;
using HomeTask.Persistence.Repositories;

namespace HomeTask.Persistence.Context
{
    public class HomeTaskDbInitializer : CreateDatabaseIfNotExists<HomeTaskDbContext>
    {
        protected override void Seed(HomeTaskDbContext context)
        {
            var id = AddClient("Нижний Новгород", true, context);
            AddOrder(id, "Большая Покровская 1", context);
            AddOrder(id, "Площадь Лядова", context);
            AddOrder(id, "Проспект Гагарина", context);

            AddClient("Санкт-Петербург", false, context);
            id = AddClient("Москва", true, context);

            AddOrder(id, "Кремль", context);
        }

        private static int AddClient(string address, bool vip, HomeTaskDbContext context)
        {
            var clientRepository = CreateRepository<Client>(context);

            var client = clientRepository.FirstOrDefault(ClientSpecifications.ByAddress(address));

            if (client == null)
            {
                client = new Client { Address = address, VIP = vip };
                clientRepository.Insert(client);

                context.SaveChanges();
            }

            return client.Id;
        }

        private static void AddOrder(int clientId, string description, HomeTaskDbContext context)
        {
            var clientRepository = CreateRepository<Client>(context);
            var orderRepository = CreateRepository<Order>(context);

            var client = clientRepository.FirstOrDefault(ClientSpecifications.ById(clientId));
            if (client != null)
            {
                var order = orderRepository.FirstOrDefault(OrderSpecifications.ByDescription(description));
                if (order == null)
                {
                    order = new Order { Description = description, ClientId = clientId };
                    orderRepository.Insert(order);
                    context.SaveChanges();
                }
            }
        }

        private static IRepository<T> CreateRepository<T>(HomeTaskDbContext context) where T : class, IEntity
        {
            return new Repository<T>(context);
        }
    }
}
