using System;

namespace WordTutor.Core.Redux
{
    /// <summary>
    /// Central store for the state of the application
    /// </summary>
    public class ReduxStore<T>
    {
        // Reference to our state reducer
        private readonly IReduxReducer<T> _reducer;

        // Flag used to prevent recursive dispatching
        private bool _dispatching;

        /// <summary>
        /// Gets the current state of the application
        /// </summary>
        public T State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReduxStore{T}" class/>
        /// </summary>
        /// <param name="reducer">Reducer to use for state transformations.</param>
        /// <param name="initialState">Initial state for the application.</param>
        public ReduxStore(IReduxReducer<T> reducer, T initialState)
        {
            _reducer = reducer ?? throw new ArgumentNullException(nameof(reducer));
            State = initialState;
        }

        /// <summary>
        /// Dispatch an application message to modify application state
        /// </summary>
        /// <param name="message">Message to process.</param>
        public void Dispatch(IReduxMessage message)
        {
            if (_dispatching)
            {
                // TOCONSIDER: If this exception becomes a problem, 
                // introduce a queue to serialize message processing instead.
                throw new InvalidOperationException(
                    "Calling Dispatch() while processing Dispatch() is not permitted.");
            }

            _dispatching = true;
            try
            {
                State = _reducer.Reduce(
                    message ?? throw new ArgumentNullException(nameof(message)),
                    State);
            }
            finally
            {
                _dispatching = false;
            }
        }
    }
}
