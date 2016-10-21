using HomeTask.Infrastructure.Logging.Base;

namespace HomeTask.Infrastructure.Logging.NLog
{
    public class NLogFactory : ILoggerFactory
    {
        private readonly string _loggerName;

        public NLogFactory(string loggerName)
        {
            _loggerName = loggerName;
        }

        public ILogger Create()
        {
            return new NLogLogger(_loggerName);
        }
    }
}
