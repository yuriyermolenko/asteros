using System.ComponentModel;
using HomeTask.Domain.Contracts.Events.Base;

namespace HomeTask.Domain.Contracts.Events.Order
{
    public abstract class OrderEventBase : IEvent
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
