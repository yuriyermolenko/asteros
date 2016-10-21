namespace HomeTask.Application.TypeAdapter
{
    public interface ITypeAdapter
    {
        TResult Create<TSource, TResult>(TSource source);
        void Update<TSource, TResult>(TSource source, TResult result);
    }
}