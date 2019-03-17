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
            State = _reducer.Reduce(
                message ?? throw new ArgumentNullException(nameof(message)), 
                State);
        }
    }
}
