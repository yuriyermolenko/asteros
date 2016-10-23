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


        

        public MainViewModel(
            IClientService clientService,
            IOrderService orderService,
            ITypeAdapter typeAdapter)
        {
            _clientService = clientService;
            _orderService = orderService;
            _typeAdapter = typeAdapter;

            var clients = clientService.GetAll();

            _clientsCollection = new ObservableCollection<ClientObservable>(
                _typeAdapter.Create<IEnumerable<ClientDTO>, IEnumerable<ClientObservable>>(clients));
            _ordersCollection = new ObservableCollection<OrderObservable>();
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
        }

        private void RefreshOrdersGridForSelectedClient()
        {
            OrdersCollection = null;
            Task refreshTask = _orderService.GetForClientAsync(SelectedClient.Id)
                .ContinueWith((orders) =>
                    OrdersCollection = new ObservableCollection<OrderObservable>(
                        _typeAdapter.Create<IEnumerable<OrderDTO>, IEnumerable<OrderObservable>>(orders.Result)
                        ), TaskScheduler.FromCurrentSynchronizationContext());

            //refreshTask.Start();
        }
    }
}