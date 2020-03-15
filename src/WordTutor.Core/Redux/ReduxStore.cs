﻿using System;
using System.Collections.Generic;
using System.Linq;

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
        private readonly HashSet<ReduxSubscription<T>> _subscriptions
            = new HashSet<ReduxSubscription<T>>();

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
            if (initialStateFactory is null)
            {
                throw new ArgumentNullException(nameof(initialStateFactory));
            }

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

            foreach (var subscription in _subscriptions.ToList())
            {
                subscription.Publish(State);
            }
        }

        /// <summary>
        /// Add middleware into the processing of the store
        /// </summary>
        /// <remarks>
        /// Middleware is activated in the order added to the store.
        /// </remarks>
        /// <param name="middleware">
        /// The middleware processor to be included in the pipeline of the store.
        /// </param>
        public void AddMiddleware(IReduxMiddleware middleware)
        {
            // Nothing
        }

        /// <summary>
        /// Create a subscription to be notified of changes to our state
        /// </summary>
        /// <typeparam name="V">Type of value being monitored</typeparam>
        /// <param name="referenceReader">Reader used to access a reference from the store.</param>
        /// <param name="whenChanged">Action to invoke when the reference changes.</param>
        /// <returns>Subscription object; disposal release the subscription.</returns>
        public IDisposable SubscribeToReference<V>(
            Func<T, V?> referenceReader,
            Action<V?> whenChanged)
            where V : class, IEquatable<V>?
        {
            var subscription = new ReduxReferenceSubscription<T, V>(
                referenceReader ?? throw new ArgumentNullException(nameof(referenceReader)),
                whenChanged ?? throw new ArgumentNullException(nameof(whenChanged)),
                ReleaseSubscription);

            _subscriptions.Add(subscription);

            return subscription;
        }

        /// <summary>
        /// Create a subscription to be notified of changes to our state
        /// </summary>
        /// <typeparam name="V">Type of value being monitored</typeparam>
        /// <param name="valueReader">Reader used to access a value from the store.</param>
        /// <param name="whenChanged">Action to invoke when the value changes.</param>
        /// <returns>Subscription object; disposal release the subscription.</returns>
        public IDisposable SubscribeToValue<V>(
            Func<T, V> valueReader,
            Action<V> whenChanged)
            where V : struct, IEquatable<V>
        {
            var subscription = new ReduxValueSubscription<T, V>(
                valueReader ?? throw new ArgumentNullException(nameof(valueReader)),
                whenChanged ?? throw new ArgumentNullException(nameof(whenChanged)),
                ReleaseSubscription);

            _subscriptions.Add(subscription);

            return subscription;
        }

        public int SubscriptionCount => _subscriptions.Count;

        public void ClearSubscriptions()
            => _subscriptions.Clear();

        private void ReleaseSubscription(ReduxSubscription<T> subscription)
        {
            _subscriptions.Remove(subscription);
        }
    }
}
