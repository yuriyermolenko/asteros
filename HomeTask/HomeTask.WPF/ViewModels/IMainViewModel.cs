using System.Collections.ObjectModel;
using HomeTask.WPF.Commands;
using HomeTask.WPF.ViewModels.Observables;

namespace HomeTask.WPF.ViewModels
{
    public interface IMainViewModel
    {
        ObservableCollection<ClientObservable> ClientsCollection { get; set; }
        ObservableCollection<OrderObservable> OrdersCollection { get; set; }
        ClientObservable SelectedClient { get; set; }
        OrderObservable SelectedOrder { get; set; }
        bool ShowClientEditor { get; set; }
        bool ShowOrderEditor { get; set; }
        ClientEditorViewModel ClientEditorViewModel { get; set; }
        OrderEditorViewModel OrderEditorViewModel { get; set; }
        IAsyncCommand DeleteClient { get; }
        DelegateCommand AddClient { get; }
        IAsyncCommand DeleteOrder { get; }
        DelegateCommand AddOrder { get; }
        DelegateCommand EditOrder { get; }
    }
}