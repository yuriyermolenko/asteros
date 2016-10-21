namespace HomeTask.Domain.Aggregates.Base
{
    public interface IAggregateRoot<T>
    {
        T Id { get; set; }
    }
}