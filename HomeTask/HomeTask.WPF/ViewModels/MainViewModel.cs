using System.Collections.Generic;
using System.Collections.ObjectModel;
using HomeTask.Application.DTO.Client;
using HomeTask.Application.Factory;
using HomeTask.Application.Services.ClientAgg;
using HomeTask.Application.TypeAdapter;
using HomeTask.WPF.ViewModels.Base;
using HomeTask.WPF.ViewModels.Observables;

namespace HomeTask.WPF.ViewModels
{
    public class MainViewModel : ObservableObject, IMainViewModel
    {
        private readonly IAppServiceFactory _serviceFactory;
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
            IAppServiceFactory serviceFactory,
            ITypeAdapter typeAdapter)
        {
            _serviceFactory = serviceFactory;
            _typeAdapter = typeAdapter;

            var clients = serviceFactory.Create<IClientService>().GetAll();

            _clientsCollection = new ObservableCollection<ClientObservable>(
                    _typeAdapter.Create<IEnumerable<ClientDTO>, IEnumerable<ClientObservable>>(clients));
        }
    }
}