using System;
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

        #region Properties & Variables

    
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


        #endregion

        #region Commands

        public DelegateCommand EditDoneCommand { get; private set; }
        public DelegateCommand EditCancelCommand { get; private set; }

        #endregion


        public OrderEditorViewModel()
        {

            #region Init Commands

            EditDoneCommand = new DelegateCommand(o => FireDone());
            EditCancelCommand = new DelegateCommand(o => FireCanceled());

            #endregion
        }



        private void FireDone()
        {
            if (OnEditDone != null)
            {
                OnEditDone(this, new OrderEditorEventArgs(Order, OrderEditorState.Done));
            }
        }

        private void FireCanceled()
        {
            if (OnEditCanceled != null)
            {
                OnEditCanceled(this, new OrderEditorEventArgs(Order, OrderEditorState.Canceled));
            }
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