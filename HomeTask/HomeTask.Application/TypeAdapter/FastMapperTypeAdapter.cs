namespace HomeTask.Application.TypeAdapter
{
    public class FastMapperTypeAdapter : ITypeAdapter
    {
        public TResult Create<TSource, TResult>(TSource source)
        {
            return FastMapper.TypeAdapter.Adapt<TSource, TResult>(source);
        }

        public void Update<TSource, TResult>(TSource source, TResult result)
        {
            FastMapper.TypeAdapter.Adapt(source, result);
        }
    }
}