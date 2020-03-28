namespace WordTutor.Core.Redux
{
    public interface IReduxMiddleware
    {
        /// <summary>
        /// Intercept, filter or fork an application message, optionally passing it to the next handler in the chain
        /// </summary>
        /// <param name="message">Message to process.</param>
        /// <param name="next">Next dispatcher to invoke.</param>
        void Dispatch(IReduxMessage message, IReduxDispatcher next);
    }
}
