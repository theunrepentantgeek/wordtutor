using System;
using System.Collections.Generic;
using System.Linq;

namespace WordTutor.Core.Redux
{
    public class CompositeReduxReducer<T> : IReduxReducer<T>
    {
        private readonly List<IReduxReducer<T>> _reducers;

        public CompositeReduxReducer(IEnumerable<IReduxReducer<T>> reducers)
        {
            _reducers = reducers?.ToList()
                ?? throw new ArgumentNullException(nameof(reducers));
        }

        public T Reduce(IReduxMessage message, T currentState)
        {
            return _reducers.Aggregate(
                currentState,
                (state, reducer) => reducer.Reduce(message, state));
        }
    }
}
