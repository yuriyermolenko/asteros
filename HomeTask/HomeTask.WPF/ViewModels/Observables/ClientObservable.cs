using System.ComponentModel;
using HomeTask.WPF.ViewModels.Base;

namespace HomeTask.WPF.ViewModels.Observables
{
    [DisplayName(@"Client")]
    public class ClientObservable : ObservableObject
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

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                if (value != _address)
                {
                    OnPropertyChanging(() => Address);
                    _address = value;
                    OnPropertyChanged(() => Address);
                }
            }
        }

        private string _vip;
        public string VIP
        {
            get { return _vip; }
            set
            {
                if (value != _vip)
                {
                    OnPropertyChanging(() => VIP);
                    _vip = value;
                    OnPropertyChanged(() => VIP);
                }
            }
        }
    }
}