namespace HomeTask.Infrastructure.Logging.Base
{
    public class LoggerFactory
    {
        static ILoggerFactory _currentLogFactory;

        public static void SetCurrent(ILoggerFactory logFactory)
        {
            _currentLogFactory = logFactory;
        }

        public static ILogger CreateLog()
        {
            return _currentLogFactory?.Create();
        }
    }
}
