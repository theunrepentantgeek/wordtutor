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
    }
}
