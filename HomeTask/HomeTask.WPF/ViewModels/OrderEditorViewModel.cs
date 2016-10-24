using System;
using HomeTask.Domain.Contracts.Events.Order;
using HomeTask.WPF.Commands;
using HomeTask.Domain.Contracts.Events.Client;
using HomeTask.Infrastructure.Messaging.Base;
using HomeTask.WPF.ViewModels.Base;
using HomeTask.WPF.ViewModels.Observables;
using HomeTask.WPF.Views;

namespace HomeTask.WPF.ViewModels
{
    public class OrderEditorViewModel : ObservableObject, IEventHandler<OrderDeleted>, IEventHandler<ClientDeleted>
    {
        private OrderObservable _order;
        public OrderObservable Order
        {
            get { return _order; }
            set
            {
                if (_order != value)
                {
                    OnPropertyChanging(() => Order);
                    _order = value;
                    OnPropertyChanged(()=> Order);
                }
            }
        }

        public event EventHandler<OrderEditorEventArgs> OnEditDone;
        public event EventHandler<OrderEditorEventArgs> OnEditCanceled;

        public DelegateCommand EditDoneCommand { get; private set; }
        public DelegateCommand EditCancelCommand { get; private set; }

        public OrderEditorViewModel()
        {
            EditDoneCommand = new DelegateCommand(o => FireDone());
            EditCancelCommand = new DelegateCommand(o => FireCanceled());
        }

        private void FireDone()
        {
            OnEditDone?.Invoke(this, new OrderEditorEventArgs(Order, OrderEditorState.Done));
        }

        private void FireCanceled()
        {
            OnEditCanceled?.Invoke(this, new OrderEditorEventArgs(Order, OrderEditorState.Canceled));
        }

        public void Handle(OrderDeleted @event)
        {
            if (@event.Id == Order.Id)
            {
                FireCanceled();
            }
            
        }
        public void Handle(ClientDeleted @event)
        {
            if (@event.Id == Order.ClientId)
            {
                FireCanceled();
            }
        }
    }
}