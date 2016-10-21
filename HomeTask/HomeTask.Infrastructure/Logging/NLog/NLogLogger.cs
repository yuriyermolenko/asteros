using System;
using NLog;

namespace HomeTask.Infrastructure.Logging.NLog
{
    public class NLogLogger : Base.ILogger
    {
        private readonly Logger _logger;

        public NLogLogger(string loggerName)
        {
            _logger = LogManager.GetLogger(loggerName);
        }

        public void LogInfo(string message, params object[] args)
        {
            _logger.Info(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.Warn(message, args);
        }

        public void LogError(string message, params object[] args)
        {
            _logger.Error(message, args);
        }

        public void LogError(string message, Exception exception, params object[] args)
        {
            _logger.Error(exception, message, args);
        }

        public void Debug(string message, params object[] args)
        {
            _logger.Debug(message, args);
        }
    }
}
