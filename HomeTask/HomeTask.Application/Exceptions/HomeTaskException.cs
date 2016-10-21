using System;
using System.Runtime.Serialization;

namespace HomeTask.Application.Exceptions
{
    public class HomeTaskException : Exception
    {
        public HomeTaskException() { }

        public HomeTaskException(string message)
            : base(message) { }

        public HomeTaskException(string message, Exception innerException)
            : base(message, innerException) { }

        protected HomeTaskException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}