using System.Collections.ObjectModel;
using HomeTask.WPF.ViewModels.Observables;

namespace HomeTask.WPF.ViewModels
{
    public interface IMainViewModel
    {
        ObservableCollection<ClientObservable> ClientsCollection { get; }
        ObservableCollection<OrderObservable> OrdersCollection { get; }
        ClientObservable SelectedClient { get; } 
    }
}