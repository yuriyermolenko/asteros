namespace HomeTask.Domain.Base
{
    public interface IAggregateRoot<T>
    {
        T Id { get; set; }
    }
}