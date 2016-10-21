using HomeTask.Domain.Contracts.Events.Base;

namespace HomeTask.Infrastructure.Messaging.Base
{
    public interface IEventBus
    {
        void Publish(IEvent @event);
    }
}