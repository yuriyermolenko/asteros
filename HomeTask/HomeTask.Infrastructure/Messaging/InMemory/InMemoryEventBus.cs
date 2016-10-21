using System.Collections.Generic;
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

        public void Publish(IEvent @event)
        {
            _eventDispatcher.DispatchMessage(@event);
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