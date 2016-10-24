using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTask.Application.DTO.Client;
using HomeTask.Application.DTO.Order;
using HomeTask.Application.Services.ClientAgg;
using HomeTask.Application.Services.OrderAgg;
using HomeTask.Application.TypeAdapter;
using HomeTask.WPF.Commands;
using HomeTask.Domain.Contracts.Events.Client;
using HomeTask.Infrastructure.Messaging.Base;
using HomeTask.WPF.ViewModels.Base;
using HomeTask.WPF.ViewModels.Observables;

namespace HomeTask.WPF.ViewModels
{
    public class MainViewModel : ObservableObject, IMainViewModel, IEventHandler<ClientCreated>, IEventHandler<ClientDeleted>
    {

        #region Properties & Variables

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

        #endregion

        #region Commands

        public IAsyncCommand DeleteClient { get; private set; }
        public DelegateCommand AddClient { get; private set; }

        #endregion


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
            #region Init Commands

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
     
            #endregion


            this.PropertyChanged += (sender, args) =>
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

            eventHandlerRegistry.Register(this);
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

        public void Handle(ClientCreated @event)
        {
           // throw new System.NotImplementedException();
        }

        public void Handle(ClientDeleted @event)
        {
           // throw new System.NotImplementedException();
        }
    }
}