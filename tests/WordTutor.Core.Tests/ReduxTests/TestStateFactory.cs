using WordTutor.Core.Redux;

namespace WordTutor.Core.Tests.ReduxTests
{
    public class TestStateFactory<T> : IReduxStateFactory<T>
    {
        public TestStateFactory(T state) => State = state;

        public T State { get; }

        public T Create() => State;
    }
}
