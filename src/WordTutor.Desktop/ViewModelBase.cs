using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

        protected void UpdateProperty<T>(
            ref T member,
            T newValue,
            Action<T> whenChanged = null,
            [CallerMemberName] string property = null)
            where T : IEquatable<T>
        {
            if (member?.Equals(newValue) == true)
            {
                return;
            }

            member = newValue;
            OnPropertyChanged(property);

            whenChanged?.Invoke(newValue);
        }

        [SuppressMessage(
            "Naming",
            "CA1715:Identifiers should have correct prefix",
            Justification = "E is a good name for a generic enumeration")]
        protected void UpdateEnumProperty<E>(
            ref E member,
            E newValue,
            Action<E> whenChanged = null,
            [CallerMemberName] string property = null)
            where E : Enum
        {
            if (member?.Equals(newValue) == true)
            {
                return;
            }

            member = newValue;
            OnPropertyChanged(property);

            whenChanged?.Invoke(newValue);
        }

        public void UpdateCollection<T>(
            ObservableCollection<T> member,
            IEnumerable<T> newList,
            [CallerMemberName]string property = null            )
        {
            if (member is null)
            {
                throw new ArgumentException(
                    "Member collection should never be null",
                    nameof(member));
            }

            if (newList is null)
            {
                throw new ArgumentNullException(nameof(newList));
            }

            if (Enumerable.SequenceEqual(member, newList))
            {
                return;
            }

            member.Clear();
            foreach(var item in newList)
            {
                member.Add(item);
            }

            OnPropertyChanged(property);
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
}
