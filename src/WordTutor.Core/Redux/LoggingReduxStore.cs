using System;
using System.Diagnostics;

namespace WordTutor.Core.Redux
{
    public class LoggingReduxStore<T> : IReduxStore<T>
    {
        private readonly IReduxStore<T> _store;

        public LoggingReduxStore(IReduxStore<T> store)
        {
            _store = store;
        }

        public T State => _store.State;

        public void Dispatch(IReduxMessage message)
        {
            Debug.WriteLine($"Dispatching {message}");
            _store.Dispatch(message);
        }

        public IDisposable SubscribeToReference<V>(
            Func<T, V?> referenceReader,
            Action<V?> whenChanged)
            where V : class, IEquatable<V>?
        {
            Debug.WriteLine($"Subscribing to {typeof(V).Name}.");
            return _store.SubscribeToReference(referenceReader, whenChanged);
        }

        public IDisposable SubscribeToValue<V>(
            Func<T, V> valueReader,
            Action<V> whenChanged)
            where V : struct, IEquatable<V>
        {
            Debug.WriteLine($"Subscribing to {typeof(V).Name}.");
            return _store.SubscribeToValue(valueReader, whenChanged);
        }
    }
}
