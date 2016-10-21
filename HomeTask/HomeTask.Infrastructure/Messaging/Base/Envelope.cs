namespace HomeTask.Infrastructure.Messaging.Base
{
    public abstract class Envelope
    {
        public static Envelope<T> Create<T>(T body)
        {
            return new Envelope<T>(body);
        }
    }

    public class Envelope<T> : Envelope
    {
        public T Body { get; private set; }

        // TODO доп поля, например, для аудита

        public Envelope(T body)
        {
            Body = body;
        }

        public static implicit operator Envelope<T>(T body)
        {
            return Create(body);
        }
    }
}
