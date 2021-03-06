﻿using System;
using HomeTask.Application.Services.ClientAgg;
using HomeTask.Application.Services.OrderAgg;
using HomeTask.Application.TypeAdapter;
using HomeTask.Infrastructure.Logging.Base;
using HomeTask.Infrastructure.Logging.NLog;
using HomeTask.Infrastructure.Messaging.Base;
using HomeTask.Infrastructure.Messaging.InMemory;
using HomeTask.Persistence.Repositories;
using HomeTask.Persistence.UnitOfWork;
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

            // Any additional other configuration

            RunApplication(container);
        }

        private static Container Bootstrap()
        {
            RegisterFactories();

            // Create the container as usual.
            var container = new Container();

            // data access
            container.Register<IUnitOfWorkFactory, UnitOfWorkFactory>();

            container.Register(typeof(IRepository<>), typeof(Repository<>));

            // application
            container.Register<ITypeAdapter, FastMapperTypeAdapter>();
            container.Register<IClientService, ClientService>();
            container.Register<IOrderService, OrderService>();

            // infrastructure
            container.RegisterSingleton<InMemoryEventBus>();
            container.RegisterSingleton<IEventBus>(() => container.GetInstance<InMemoryEventBus>());
            container.RegisterSingleton<IEventHandlerRegistry>(() => container.GetInstance<InMemoryEventBus>());

            // Register your windows and view models:
            container.Register<MainView>();
            container.Register<IMainViewModel, MainViewModel>();

            container.Verify();

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