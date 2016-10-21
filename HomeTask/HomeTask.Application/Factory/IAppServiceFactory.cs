namespace HomeTask.Application.Factory
{
    public interface IAppServiceFactory
    {
        T Create<T>() where T : class;
    }
}