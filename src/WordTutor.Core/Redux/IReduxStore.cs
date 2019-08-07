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
        /// Create a subscription to be notified of changes to our state
        /// </summary>
        /// <typeparam name="V">Type of value being monitored</typeparam>
        /// <param name="reader">Reader used to access a value from the store.</param>
        /// <param name="whenChanged">Action to invoke when the value changes.</param>
        /// <returns>Subscription object; disposal release the subscription.</returns>
        IDisposable Subscribe<V>(
            Func<T, V> reader, 
            Action<V> whenChanged) 
            where V : System.IEquatable<V>;
    }
}
