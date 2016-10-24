using System;
using HomeTask.WPF.ViewModels.Observables;

namespace HomeTask.WPF.Views
{
    public enum EditorState
    {
        Done,
        Canceled,
    }

    public class ClientEditorEventArgs : EventArgs
    {
        public ClientObservable Client { get; private set; }
        public EditorState State { get; private set; }

        public ClientEditorEventArgs(ClientObservable client, EditorState state)
        {
            Client = client;
            State = state;
        }
    }
}
