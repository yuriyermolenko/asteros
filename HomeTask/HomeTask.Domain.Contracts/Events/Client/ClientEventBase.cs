using HomeTask.Domain.Contracts.Events.Base;

namespace HomeTask.Domain.Contracts.Events.Client
{
    public abstract class ClientEventBase : IEvent
    {
        public int ClientId { get; set; }
        public string Address { get; set; }
        public bool VIP { get; set; }
    }
}
