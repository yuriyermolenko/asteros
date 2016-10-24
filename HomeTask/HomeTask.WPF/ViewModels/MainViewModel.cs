using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HomeTask.Application.DTO.Client;
using HomeTask.Application.DTO.Order;
using HomeTask.Application.Services.ClientAgg;
using HomeTask.Application.Services.OrderAgg;
using HomeTask.Application.TypeAdapter;
using HomeTask.WPF.Commands;
using HomeTask.Infrastructure.Messaging.Base;
using HomeTask.WPF.ViewModels.Base;
using HomeTask.WPF.ViewModels.Observables;

namespace HomeTask.WPF.ViewModels
{
    public class MainViewModel : ObservableObject, IMainViewModel
    {
        private readonly IClientService _clientService;
        private readonly IOrderService _orderService;
        private readonly ITypeAdapter _typeAdapter;

        private ObservableCollection<ClientObservable> _clientsCollection;
        public ObservableCollection<ClientObservable> ClientsCollection
        {
            get { return _clientsCollection; }
            set
            {
                if (_clientsCollection != value)
                {
                    OnPropertyChanging(() => ClientsCollection);
                    _clientsCollection = value;
                    OnPropertyChanged(() => ClientsCollection);
                }
            }
        }

        private ObservableCollection<OrderObservable> _ordersCollection;
        public ObservableCollection<OrderObservable> OrdersCollection
        {
            get { return _ordersCollection; }
            set
            {
                if (_ordersCollection != value)
                {
                    OnPropertyChanging(() => OrdersCollection);
                    _ordersCollection = value;
                    OnPropertyChanged(() => OrdersCollection);
                }
            }
        }

        private ClientObservable _selectedClient;
        public ClientObservable SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                if (_selectedClient != value)
                {
                    OnPropertyChanging(() => SelectedClient);
                    _selectedClient = value;
                    OnPropertyChanged(()=> SelectedClient);
                }
            }
        }

        private OrderObservable _selectedOrder;
        public OrderObservable SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                if (_selectedOrder != value)
                {
                    OnPropertyChanging(() => SelectedOrder);
                    _selectedOrder = value;
                    OnPropertyChanged(() => SelectedOrder);
                }
            }
        }

        private bool _showClientEditor;
        public bool ShowClientEditor
        {
            get { return _showClientEditor; }
            set
            {
                if (_showClientEditor != value)
                {
                    OnPropertyChanging(() => ShowClientEditor);
                    _showClientEditor = value;
                    OnPropertyChanged(() => ShowClientEditor);
                }
            }
        }

        private bool _showOrderEditor;
        public bool ShowOrderEditor
        {
            get { return _showOrderEditor; }
            set
            {
                if (_showOrderEditor != value)
                {
                    OnPropertyChanging(() => ShowOrderEditor);
                    _showOrderEditor = value;
                    OnPropertyChanged(() => ShowOrderEditor);
                }
            }
        }

        private ClientEditorViewModel _clientEditorViewModel;
        public ClientEditorViewModel ClientEditorViewModel
        {
            get { return _clientEditorViewModel; }
            set
            {
                if (_clientEditorViewModel != value)
                {
                    OnPropertyChanging(() => ClientEditorViewModel);
                    _clientEditorViewModel = value;
                    OnPropertyChanged(() => ClientEditorViewModel);
                }
            }
        }

        private OrderEditorViewModel _orderEditorViewModel;
        public OrderEditorViewModel OrderEditorViewModel
        {
            get { return _orderEditorViewModel; }
            set
            {
                if (_orderEditorViewModel != value)
                {
                    OnPropertyChanging(() => OrderEditorViewModel);
                    _orderEditorViewModel = value;
                    OnPropertyChanged(() => OrderEditorViewModel);
                }
            }
        }

        public IAsyncCommand DeleteClient { get; }
        public DelegateCommand AddClient { get; }
        public IAsyncCommand DeleteOrder { get; }
        public DelegateCommand AddOrder { get; }
        public DelegateCommand EditOrder { get; }

        public MainViewModel(
            IClientService clientService,
            IOrderService orderService,
            ITypeAdapter typeAdapter,
            IEventHandlerRegistry eventHandlerRegistry)
        {
            _clientService = clientService;
            _orderService = orderService;
            _typeAdapter = typeAdapter;

            var clients = clientService.GetAll();

            _clientsCollection = new ObservableCollection<ClientObservable>(
                _typeAdapter.Create<IEnumerable<ClientDTO>, IEnumerable<ClientObservable>>(clients));
            _ordersCollection = new ObservableCollection<OrderObservable>();

            ShowClientEditor = false;
            ShowOrderEditor = false;

            DeleteClient = AsyncCommand.Create(() =>
            {
                if (SelectedClient != null)
                {
                    return Task.Run(() =>
                    {
                        _clientService.DeleteAsync(SelectedClient.Id).ContinueWith( (t) =>
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(
                                () =>
                                {
                                    ClientsCollection.Remove(SelectedClient);
                                    SelectedClient = null;
                                });
                            
                        },  TaskContinuationOptions.OnlyOnRanToCompletion);
                        
                    });
                }
                return Task.CompletedTask;
            });

            AddClient = new DelegateCommand((o) =>
            {
                ClientEditorViewModel = new ClientEditorViewModel() { Client = new ClientObservable() };
                ClientEditorViewModel.OnEditCanceled += (sender, args) =>
                {
                    ShowClientEditor = false;
                };
                ClientEditorViewModel.OnEditDone += (sender, args) =>
                {
                    if (ClientEditorViewModel.Client != null)
                    {
                        Task.Run(() =>
                        {
                            _clientService.CreateAsync(new CreateClientRequest(ClientEditorViewModel.Client.Address, ClientEditorViewModel.Client.VIP)).ContinueWith((t) =>
                            {
                                ClientEditorViewModel.Client.Id = t.Result;
                                System.Windows.Application.Current.Dispatcher.Invoke(
                                    () =>
                                    {
                                        ClientsCollection.Add(ClientEditorViewModel.Client);
                                        SelectedClient = ClientEditorViewModel.Client;
                                        ShowClientEditor = false;
                                    });

                            }, TaskContinuationOptions.OnlyOnRanToCompletion);
                        });
                    }
                };
                ShowClientEditor = true;
            });
     
            DeleteOrder = AsyncCommand.Create(() =>
            {
                if (SelectedOrder != null)
                {
                    return Task.Run(() =>
                    {
                        _orderService.DeleteAsync(SelectedOrder.Id).ContinueWith((t) =>
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(
                                () =>
                                {
                                    OrdersCollection.Remove(SelectedOrder);
                                    SelectedOrder = null;
                                });

                        }, TaskContinuationOptions.OnlyOnRanToCompletion);

                    });
                }
                return Task.CompletedTask;
            });

            AddOrder= new DelegateCommand((o) =>
            {
                OrderEditorViewModel = new OrderEditorViewModel() { Order = new OrderObservable() {ClientId = SelectedClient.Id} };
                OrderEditorViewModel.OnEditCanceled += (sender, args) =>
                {
                    ShowOrderEditor = false;
                };
                OrderEditorViewModel.OnEditDone += (sender, args) =>
                {
                    if (OrderEditorViewModel.Order != null)
                    {
                        Task.Run(() =>
                        {
                            _orderService.CreateAsync(new CreateOrderRequest(OrderEditorViewModel.Order.Description, OrderEditorViewModel.Order.ClientId)).ContinueWith((t) =>
                            {
                                OrderEditorViewModel.Order.Id = t.Result;
                                System.Windows.Application.Current.Dispatcher.Invoke(
                                    () =>
                                    {
                                        OrdersCollection.Add(OrderEditorViewModel.Order);
                                        SelectedOrder = OrderEditorViewModel.Order;
                                        ShowOrderEditor = false;
                                    });

                            }, TaskContinuationOptions.OnlyOnRanToCompletion);
                        });
                    }
                };
                eventHandlerRegistry.Register(OrderEditorViewModel);
                ShowOrderEditor = true;
            });

            EditOrder = new DelegateCommand((o) =>
            {
                OrderEditorViewModel = new OrderEditorViewModel() { Order = new OrderObservable() { ClientId = SelectedOrder.ClientId, Id = SelectedOrder.Id, Description = SelectedOrder.Description}};
                OrderEditorViewModel.OnEditCanceled += (sender, args) =>
                {
                    ShowOrderEditor = false;
                };
                OrderEditorViewModel.OnEditDone += (sender, args) =>
                {
                    if (OrderEditorViewModel.Order != null)
                    {
                        Task.Run(() =>
                        {
                            _orderService.UpdateAsync(new UpdateOrderRequest(OrderEditorViewModel.Order.Id, OrderEditorViewModel.Order.Description, OrderEditorViewModel.Order.ClientId)).ContinueWith((t) =>
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke(
                                    () =>
                                    {
                                        var order =
                                            OrdersCollection.FirstOrDefault((ord) => ord.Id == OrderEditorViewModel.Order.Id);
                                        if (order != null)
                                        {
                                           var idx = OrdersCollection.IndexOf(order);
                                            if (idx > -1)
                                            {
                                                OrdersCollection.RemoveAt(idx);
                                                OrdersCollection.Insert(idx, OrderEditorViewModel.Order);
                                            }
                                        }
                                        SelectedOrder = OrderEditorViewModel.Order;
                                        ShowOrderEditor = false;
                                    });

                            }, TaskContinuationOptions.OnlyOnRanToCompletion);
                        });
                    }
                };
                eventHandlerRegistry.Register(OrderEditorViewModel);
                ShowOrderEditor = true;
            });

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "SelectedClient")
                {
                    if (SelectedClient != null)
                    {
                       RefreshOrdersGridForSelectedClient();
                    }
                    else
                    {
                        OrdersCollection.Clear();
                    }
                }
            };
        }

        private void RefreshOrdersGridForSelectedClient()
        {
            OrdersCollection = null;
            _orderService.GetForClientAsync(SelectedClient.Id)
                .ContinueWith((orders) =>
                    OrdersCollection = new ObservableCollection<OrderObservable>(
                        _typeAdapter.Create<IEnumerable<OrderDTO>, IEnumerable<OrderObservable>>(orders.Result)
                        ), TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}