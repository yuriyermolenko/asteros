using System.Collections.Generic;
using HomeTask.Domain.Aggregates.Base;
using HomeTask.Domain.Aggregates.OrderAgg;

namespace HomeTask.Domain.Aggregates.ClientAgg
{
    public class Client : IAggregateRoot<int>
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public bool VIP { get; set; }

        // -> navigation properties
        public virtual ICollection<Order> Orders { get; set; }

        public Client()
        {
            this.Orders = new List<Order>();
        }
    }
}