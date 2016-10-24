using System.ComponentModel;
using HomeTask.WPF.ViewModels.Base;

namespace HomeTask.WPF.ViewModels.Observables
{
    [DisplayName(@"Order")]
    public class OrderObservable : ObservableObject
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    OnPropertyChanging(() => Id);
                    _id = value;
                    OnPropertyChanged(() => Id);
                }
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    OnPropertyChanging(() => Description);
                    _description = value;
                    OnPropertyChanged(() => Description);
                }
            }
        }

        private int _clientId;
        public int ClientId
        {
            get { return _clientId; }
            set
            {
                if (value != _clientId)
                {
                    OnPropertyChanging(() => ClientId);
                    _clientId = value;
                    OnPropertyChanged(() => ClientId);
                }
            }
        }
    }
}