using HomeTask.Application.Factory;

namespace HomeTask.WPF.ViewModels
{
    public class MainViewModel : IMainViewModel
    {
        private readonly IAppServiceFactory _serviceFactory;

        public MainViewModel(IAppServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
    }
}