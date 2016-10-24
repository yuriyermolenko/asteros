using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace HomeTask.WPF.ViewModels.Base
{
    public class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        [Browsable(false)]
        public bool NotificationSuspended
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (NotificationSuspended == false)
            {
                var handler = PropertyChanged;
                handler?.Invoke(this, args);
            }
        }

        protected virtual void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;

            var body = lambda.Body as UnaryExpression;
            if (body != null)
            {
                var unaryExpression = body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            OnPropertyChanged(memberExpression.Member.Name);
        }


        protected virtual void OnPropertyChanging(string propertyName)
        {
            OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanging(PropertyChangingEventArgs args)
        {
            if (NotificationSuspended == false)
            {
                var handler = PropertyChanging;
                handler?.Invoke(this, args);
            }
        }

        protected virtual void OnPropertyChanging<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;

            var body = lambda.Body as UnaryExpression;
            if (body != null)
            {
                var unaryExpression = body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            OnPropertyChanging(memberExpression.Member.Name);
        }
    }
}

