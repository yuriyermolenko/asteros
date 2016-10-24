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
using HomeTask.WPF.Commands;
using HomeTask.Domain.Contracts.Events.Client;
using HomeTask.Infrastructure.Messaging.Base;
using HomeTask.WPF.ViewModels.Base;
using HomeTask.WPF.ViewModels.Observables;
using HomeTask.WPF.Views;

namespace HomeTask.WPF.ViewModels
{
    public class ClientEditorViewModel : ObservableObject, IEventHandler<ClientDeleted>
    {

        #region Properties & Variables

    
        private ClientObservable _client;
        public ClientObservable Client
        {
            get { return _client; }
            set
            {
                if (_client != value)
                {
                    OnPropertyChanging(() => Client);
                    _client = value;
                    OnPropertyChanged(()=> Client);
                }
            }
        }

        public event EventHandler<ClientEditorEventArgs> OnEditDone;
        public event EventHandler<ClientEditorEventArgs> OnEditCanceled;


        #endregion

        #region Commands

        public DelegateCommand EditDoneCommand { get; private set; }
        public DelegateCommand EditCancelCommand { get; private set; }

        #endregion


        public ClientEditorViewModel()
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
                OnEditDone(this, new ClientEditorEventArgs(Client, EditorState.Done));
            }
        }

        private void FireCanceled()
        {
            if (OnEditCanceled != null)
            {
                OnEditCanceled(this, new ClientEditorEventArgs(Client, EditorState.Canceled));
            }
        }


        public void Handle(ClientCreated @event)
        {
            //throw new System.NotImplementedException();
        }

        public void Handle(ClientDeleted @event)
        {
            if (@event.Id == Client.Id)
            {
                FireCanceled();
            }
        }
    }
}