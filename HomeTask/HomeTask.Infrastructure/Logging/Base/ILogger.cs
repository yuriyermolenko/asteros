using System;

namespace HomeTask.Infrastructure.Logging.Base
{
    public interface ILogger
    {
        void LogInfo(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(string message, params object[] args);
        void LogError(string message, Exception exception, params object[] args);
        void Debug(string message, params object[] args);
    }
}
