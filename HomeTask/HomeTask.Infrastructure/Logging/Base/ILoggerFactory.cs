namespace HomeTask.Infrastructure.Logging.Base
{
    public interface ILoggerFactory
    {
        ILogger Create();
    }
}
