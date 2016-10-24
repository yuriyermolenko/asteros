using System;
using HomeTask.WPF.ViewModels.Observables;

namespace HomeTask.WPF.Views
{
    public enum OrderEditorState
    {
        Done,
        Canceled,
    }

    public class OrderEditorEventArgs : EventArgs
    {
        public OrderObservable Order { get; private set; }
        public OrderEditorState State { get; private set; }

        public OrderEditorEventArgs(OrderObservable order, OrderEditorState state)
        {
            Order = order;
            State = state;
        }
    }
}
