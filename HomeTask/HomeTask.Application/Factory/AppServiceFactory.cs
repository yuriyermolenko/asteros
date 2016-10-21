using SimpleInjector;

namespace HomeTask.Application.Factory
{
    public class AppServiceFactory : IAppServiceFactory
    {
        private readonly Container _container;

        public AppServiceFactory(Container container)
        {
            _container = container;
        }

        public T Create<T>() where T : class 
        {
            return _container.GetInstance<T>();
        }
    }
}