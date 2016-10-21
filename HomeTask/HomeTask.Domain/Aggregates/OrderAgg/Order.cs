using HomeTask.Domain.Aggregates.ClientAgg;
using HomeTask.Domain.Base;

namespace HomeTask.Domain.Aggregates.OrderAgg
{
    public class Order : IAggregateRoot<int>
    {
        public int Id { get; set; }
        public string Description { get; set; }

        // -> navigation properties
        public virtual Client Client { get; set; }
    }
}