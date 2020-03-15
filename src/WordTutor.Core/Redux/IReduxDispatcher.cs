namespace WordTutor.Core.Redux
{
    public interface IReduxDispatcher
    {
        /// <summary>
        /// Dispatch an application message
        /// </summary>
        /// <param name="message">Message to process.</param>
        void Dispatch(IReduxMessage message);
    }
}
