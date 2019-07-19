namespace WordTutor.Core.Redux
{
    public interface IReduxReducer<T>
    {
        /// <summary>
        /// Apply a message to the current state and generate a new state
        /// </summary>
        /// <param name="message">Message representing the requested change to the state.</param>
        /// <param name="currentState">Current state, before transformation.</param>
        /// <returns>Updated state, after the requested transformation.</returns>
        T Reduce(IReduxMessage message, T currentState);
    }
}
