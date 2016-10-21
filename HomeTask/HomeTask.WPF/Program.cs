using System;
using System.Data.Entity;
using HomeTask.Application.Factory;
using HomeTask.Application.Services.ClientAgg;
using HomeTask.Application.Services.OrderAgg;
using HomeTask.Application.TypeAdapter;
using HomeTask.Infrastructure.Logging.Base;
using HomeTask.Infrastructure.Logging.NLog;
using HomeTask.Infrastructure.Messaging.Base;
using HomeTask.Infrastructure.Messaging.InMemory;
using HomeTask.Persistence;
using HomeTask.Persistence.Repositories;
using HomeTask.Persistence.UnitOfWork;
using HomeTask.WPF.ViewModels;
using HomeTask.WPF.Views;
using SimpleInjector;
using SimpleInjector.Diagnostics;

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
            RegisterFactories();

            // Create the container as usual.
            var container = new Container();

            // data access
            //container.Register<IDatabaseInitializer<HomeTaskDbContext>, HomeTaskDbInitializer>();
            container.Register<HomeTaskDbContext>();

            container.Register<DbContext, HomeTaskDbContext>();
            container.Register<DbContextAdapter>();

            container.Register<IObjectSetFactory, DbContextAdapter>();
            container.Register<IObjectContext, DbContextAdapter>();
            container.Register<IUnitOfWork, UnitOfWork>();

            container.Register(typeof(IRepository<>), typeof(Repository<>));

            // application
            container.Register<IAppServiceFactory>(() => new AppServiceFactory(container));
            container.Register<ITypeAdapter, FastMapperTypeAdapter>();
            container.Register<IClientService, ClientService>();
            container.Register<IOrderService, OrderService>();

            // infrastructure
            container.Register<IEventBus, InMemoryEventBus>();

            // Register your windows and view models:
            container.Register<MainView>();
            container.Register<IMainViewModel, MainViewModel>();

            //Registration registration1 = container.GetRegistration(typeof(DbContext)).Registration;
            //registration1.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Dispose called by application code");

            //Registration registration2 = container.GetRegistration(typeof(IObjectSetFactory)).Registration;
            //registration2.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Dispose called by application code");

            //Registration registration3 = container.GetRegistration(typeof(IObjectContext)).Registration;
            //registration3.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Dispose called by application code");

            //container.Verify();

            return container;
        }

        private static void RegisterFactories()
        {
            LoggerFactory.SetCurrent(new NLogFactory("filelogger"));
        }

        private static void RunApplication(Container container)
        {
            var app = new App();
            var mainWindow = container.GetInstance<MainView>();
            app.Run(mainWindow);
        }
    }
}