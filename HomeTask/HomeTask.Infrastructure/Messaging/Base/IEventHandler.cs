using HomeTask.Domain.Contracts.Events.Base;

namespace HomeTask.Infrastructure.Messaging.Base
{
    public interface IEventHandler
    {
    }

    public interface IEventHandler<in T> : IEventHandler
        where T : IEvent
    {
        void Handle(T @event);
    }

    public interface IEnvelopedEventHandler<T> : IEventHandler
        where T : IEvent
    {
        void Handle(Envelope<T> envelope);
    }
}