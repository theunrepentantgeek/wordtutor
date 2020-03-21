using System;
using WordTutor.Core.Redux;

namespace WordTutor.Core.Tests.Fakes
{
    public sealed class FakeReducer<T> : IReduxReducer<T>
    {
        private Func<IReduxMessage, T, T> _reduce;

        public FakeReducer(Func<IReduxMessage, T, T> reduce)
            => _reduce = reduce ?? throw new ArgumentNullException(nameof(reduce));

        T IReduxReducer<T>.Reduce(IReduxMessage message, T currentState)
            => _reduce(message, currentState);
    }
}
