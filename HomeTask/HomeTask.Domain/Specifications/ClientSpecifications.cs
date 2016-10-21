using System;
using HomeTask.Domain.Aggregates.ClientAgg;
using HomeTask.Domain.Specifications.Base;

namespace HomeTask.Domain.Specifications
{
    public static class ClientSpecifications
    {
        public static Specification<Client> ById(int clientId)
        {
            return new DirectSpecification<Client>(c => c.Id == clientId);
        }

        public static Specification<Client> Any()
        {
            return new TrueSpecification<Client>();
        }

        public static Specification<Client> ByAddress(string address)
        {
            return new DirectSpecification<Client>(c => c.Address == address);
        }
    }
}