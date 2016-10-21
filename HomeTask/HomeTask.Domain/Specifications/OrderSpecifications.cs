using HomeTask.Domain.Aggregates.OrderAgg;
using HomeTask.Domain.Specifications.Base;

namespace HomeTask.Domain.Specifications
{
    public static class OrderSpecifications
    {
        public static Specification<Order> ById(int orderId)
        {
            return new DirectSpecification<Order>(c => c.Id == orderId);
        }

        public static Specification<Order> ByClientId(int clientId)
        {
            return new DirectSpecification<Order>(c => c.ClientId == clientId);
        }
    }
}