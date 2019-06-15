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
    }
}
