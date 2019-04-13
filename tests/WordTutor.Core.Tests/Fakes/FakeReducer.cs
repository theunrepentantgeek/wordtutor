using System;
using WordTutor.Core.Redux;

namespace WordTutor.Core.Tests.Fakes
{
    public sealed class FakeReducer<T> : IReduxReducer<T>
    {
        public Func<IReduxMessage, T, T> Reduce { get; set; } = (_, s) => s;

        T IReduxReducer<T>.Reduce(IReduxMessage message, T currentState)
            => Reduce(message, currentState);
    }
}
