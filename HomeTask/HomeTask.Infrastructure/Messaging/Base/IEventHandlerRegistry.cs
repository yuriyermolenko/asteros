namespace HomeTask.Infrastructure.Messaging.Base
{
    public interface IEventHandlerRegistry
    {
        void Register(IEventHandler handler);
    }
}