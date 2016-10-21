using System;
using System.Runtime.Serialization;

namespace HomeTask.Application.Exceptions
{
    public class NotFoundException : HomeTaskException
    {
        public NotFoundException() { }

        public NotFoundException(string message)
            : base(message) { }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
