using System;
using System.Collections.Generic;

namespace WordTutor.Core.Redux
{
    /// <summary>
    /// Central store for the state of the application
    /// </summary>
    public class ReduxStore<T> : IReduxStore<T>
    {
        // Reference to our state reducer
        private readonly IReduxReducer<T> _reducer;

        // Flag used to prevent recursive dispatching
        private bool _dispatching;

        // Set of all our current subscriptions
        private HashSet<ReduxSubscription<T>> _subscriptions = new HashSet<ReduxSubscription<T>>();

        /// <summary>
        /// Gets the current state of the application
        /// </summary>
        public T State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReduxStore{T}" class/>
        /// </summary>
        /// <param name="reducer">Reducer to use for state transformations.</param>
        /// <param name="initialStateFactory">Factory used to create the initial state of the application.</param>
        public ReduxStore(
            IReduxReducer<T> reducer, 
            IReduxStateFactory<T> initialStateFactory)
        {
            _reducer = reducer ?? throw new ArgumentNullException(nameof(reducer));
            State = initialStateFactory.Create();
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

            foreach (var subscription in _subscriptions)
            {
                subscription.Publish(State);
            }
        }

        /// <summary>
        /// Create a subscription to be notified of changes to our state
        /// </summary>
        /// <typeparam name="V">Type of value being monitored</typeparam>
        /// <param name="reader">Reader used to access a value from the store.</param>
        /// <param name="whenChanged">Action to invoke when the value changes.</param>
        /// <returns>Subscription object; disposal release the subscription.</returns>
        public IDisposable Subscribe<V>(
            Func<T, V> reader,
            Action<V> whenChanged)
            where V: IEquatable<V>
        {
            var subscription = new ReduxSubscription<T, V>(
                reader ?? throw new ArgumentNullException(nameof(reader)),
                whenChanged ?? throw new ArgumentNullException(nameof(whenChanged)),
                ReleaseSubscription);

            _subscriptions.Add(subscription);

            return subscription;
        }

        public int SubscriptionCount => _subscriptions.Count;

        private void ReleaseSubscription(ReduxSubscription<T> subscription)
        {
            _subscriptions.Remove(subscription);
        }
    }
}
