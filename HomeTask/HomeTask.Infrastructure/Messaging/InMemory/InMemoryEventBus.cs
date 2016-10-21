using System.Collections.Generic;
using System.Linq;
using HomeTask.Domain.Contracts.Events.Base;
using HomeTask.Infrastructure.Messaging.Base;

namespace HomeTask.Infrastructure.Messaging.InMemory
{
    public class InMemoryEventBus : IEventBus, IEventHandlerRegistry
    {
        private readonly EventDispatcher _eventDispatcher;

        public InMemoryEventBus()
        {
            _eventDispatcher = new EventDispatcher();
        }

        public void Publish(Envelope<IEvent> @event)
        {
            _eventDispatcher.DispatchMessage(@event.Body);
        }

        public void Publish(IEnumerable<Envelope<IEvent>> events)
        {
            _eventDispatcher.DispatchMessages(events.Select(evt => evt.Body));
        }

        public void Register(IEventHandler handler)
        {
            _eventDispatcher.Register(handler);
        }

        public void RegisterAll(IEnumerable<IEventHandler> handlers)
        {
            foreach (var handler in handlers)
            {
                _eventDispatcher.Register(handler);
            }
        }
    }
}