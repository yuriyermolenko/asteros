using System;
using HomeTask.Application.Factory;
using HomeTask.WPF.ViewModels;
using HomeTask.WPF.Views;
using SimpleInjector;

namespace HomeTask.WPF
{
    public class Program
    {
        [STAThread]
        private static void Main()
        {
            var container = Bootstrap();

            // Any additional other configuration, e.g. of your desired MVVM toolkit.

            RunApplication(container);
        }

        private static Container Bootstrap()
        {
            // Create the container as usual.
            var container = new Container();

            // data access
            container.Register<IAppServiceFactory>(() => new AppServiceFactory(container));

            // application

            // Register your windows and view models:
            container.Register<MainView>();
            container.Register<IMainViewModel, MainViewModel>();

            container.Verify();

            return container;
        }

        private static void RunApplication(Container container)
        {
            var app = new App();
            var mainWindow = container.GetInstance<MainView>();
            app.Run(mainWindow);
        }
    }
}