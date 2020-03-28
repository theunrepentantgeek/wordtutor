using System;

namespace WordTutor.Core.Redux
{
    public interface IReduxStore<T>
    {
        /// <summary>
        /// Gets the current state of the application
        /// </summary>
        T State { get; }

        /// <summary>
        /// Dispatch an application message to modify application state
        /// </summary>
        /// <param name="message">Message to process.</param>
        void Dispatch(IReduxMessage message);

        /// <summary>
        /// Add middleware into the processing of the store
        /// </summary>
        /// <remarks>
        /// Middleware is activated in the order added to the store.
        /// </remarks>
        /// <param name="middleware">
        /// The middleware processor to be included in the pipeline of the store.
        /// </param>
        void AddMiddleware(IReduxMiddleware middleware);

        /// <summary>
        /// Create a subscription to be notified of changes to our state
        /// </summary>
        /// <typeparam name="V">Type of value being monitored</typeparam>
        /// <param name="referenceReader">Reader used to access a value from the store.</param>
        /// <param name="whenChanged">Action to invoke when the value changes.</param>
        /// <returns>Subscription object; disposal release the subscription.</returns>
        IDisposable SubscribeToReference<V>(
            Func<T, V?> referenceReader,
            Action<V?> whenChanged)
            where V : class, IEquatable<V>?;

        /// <summary>
        /// Create a subscription to be notified of changes to our state
        /// </summary>
        /// <typeparam name="V">Type of value being monitored</typeparam>
        /// <param name="valueReader">Reader used to access a value from the store.</param>
        /// <param name="whenChanged">Action to invoke when the value changes.</param>
        /// <returns>Subscription object; disposal release the subscription.</returns>
        IDisposable SubscribeToValue<V>(
            Func<T, V> valueReader,
            Action<V> whenChanged)
            where V : struct, IEquatable<V>;
    }
}
