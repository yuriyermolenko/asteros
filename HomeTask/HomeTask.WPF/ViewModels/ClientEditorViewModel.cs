using System;
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

        public DelegateCommand EditDoneCommand { get; private set; }
        public DelegateCommand EditCancelCommand { get; private set; }

        public ClientEditorViewModel()
        {
            #region Init Commands

            EditDoneCommand = new DelegateCommand(o => FireDone());
            EditCancelCommand = new DelegateCommand(o => FireCanceled());

            #endregion
        } 

        public void Handle(ClientDeleted @event)
        {
            if (@event.Id == Client.Id)
            {
                FireCanceled();
            }
        }

        private void FireDone()
        {
            OnEditDone?.Invoke(this, new ClientEditorEventArgs(Client, EditorState.Done));
        }

        private void FireCanceled()
        {
            OnEditCanceled?.Invoke(this, new ClientEditorEventArgs(Client, EditorState.Canceled));
        }
    }
}