namespace WordTutor.Core.Redux
{
    /// <summary>
    /// Factory interface used to create the initial state for a Redux Store
    /// </summary>
    /// <typeparam name="T">Type of state expected by the store.</typeparam>
    public interface IReduxStateFactory<T>
    {
        T Create();
    }
}
