using System.Collections.Generic;
using System.Collections.ObjectModel;
using HomeTask.Application.DTO.Client;
using HomeTask.Application.Services.ClientAgg;
using HomeTask.Application.TypeAdapter;
using HomeTask.WPF.ViewModels.Base;
using HomeTask.WPF.ViewModels.Observables;

namespace HomeTask.WPF.ViewModels
{
    public class MainViewModel : ObservableObject, IMainViewModel
    {
        private readonly IClientService _clientService;
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


        public MainViewModel(
            IClientService clientService,
            ITypeAdapter typeAdapter)
        {
            _clientService = clientService;
            _typeAdapter = typeAdapter;

            var clients = clientService.GetAll();

            _clientsCollection = new ObservableCollection<ClientObservable>(
                _typeAdapter.Create<IEnumerable<ClientDTO>, IEnumerable<ClientObservable>>(clients));
        }
    }
}