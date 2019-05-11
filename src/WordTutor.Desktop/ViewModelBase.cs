using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace WordTutor.Desktop
{
    public interface IViewModel<out T>
    {
        T Model { get; }
    }

    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        // Captured synchronization context so we always dispatch events on the UX thread
        private readonly SynchronizationContext _synchronizationContext;

        public event PropertyChangedEventHandler PropertyChanged;

        protected ViewModelBase()
        {
            // We always want our PropertyChanged event to be fired on the UX thread
            _synchronizationContext = SynchronizationContext.Current;
        }

        protected bool UpdateProperty(
            ref int member,
            int newValue,
            [CallerMemberName] string property = null)
        {
            if (member == newValue)
            {
                return false;
            }

            member = newValue;
            OnPropertyChanged(property);

            return true;
        }

        [SuppressMessage(
            "Naming",
            "CA1715:Identifiers should have correct prefix", 
            Justification = "E is a good name for a generic enumeration")]
        protected bool UpdateProperty<E>(
            ref E member,
            E newValue,
            [CallerMemberName] string property = null)
            where E : Enum
        {
            if (Equals(member, newValue))
            {
                return false;
            }

            member = newValue;
            OnPropertyChanged(property);

            return true;
        }

        protected bool UpdateProperty(
            ref string member,
            string newValue,
            [CallerMemberName] string property = null)
        {
            if (member == newValue)
            {
                return false;
            }

            member = newValue;
            OnPropertyChanged(property);

            return true;
        }

        protected bool UpdateProperty(
            ref DateTimeOffset member,
            DateTimeOffset newValue,
            [CallerMemberName] string property = null)
        {
            if (member == newValue)
            {
                return false;
            }

            member = newValue;
            OnPropertyChanged(property);

            return true;
        }

        protected bool UpdateProperty(
            ref TimeSpan member,
            TimeSpan newValue,
            [CallerMemberName] string property = null)
        {
            if (Equals(member, newValue))
            {
                return false;
            }

            member = newValue;
            OnPropertyChanged(property);

            return true;
        }

        private void OnPropertyChanged(string property)
        {
            if (SynchronizationContext.Current == _synchronizationContext
                || _synchronizationContext == null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }
            else
            {
                _synchronizationContext.Send(
                    _ => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property)),
                    state: null);
            }
        }
    }

    public abstract class ViewModelBase<T> : ViewModelBase, IViewModel<T>
    {
        // Reference to our current model
        private T _model;

        /// <summary>
        /// Gets or sets a reference to the current model held by this ViewModel
        /// </summary>
        public T Model
        {
            get => _model;
            set
            {
                _model = value;
                ModelUpdated(value);
            }
        }

        /// <summary>
        /// Update properties when the Model is changed
        /// </summary>
        /// <param name="model"></param>
        protected abstract void ModelUpdated(T model);
    }
}
