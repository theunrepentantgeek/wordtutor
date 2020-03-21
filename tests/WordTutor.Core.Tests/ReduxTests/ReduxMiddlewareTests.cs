using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using WordTutor.Core.Redux;
using WordTutor.Core.Tests.Fakes;
using Xunit;

namespace WordTutor.Core.Tests.ReduxTests
{
    public class ReduxMiddlewareTests
    {
        private readonly TestStateFactory<int> stateFactory = new TestStateFactory<int>(0);
        private readonly FakeReducer<int> _incrementingReducer;
        private readonly ReduxStore<int> _store;

        private readonly InterceptingReduxMiddleware _passThroughMiddleware
            = new InterceptingReduxMiddleware(
                (message, next) => next.Dispatch(message));

        public ReduxMiddlewareTests()
        {
            _incrementingReducer = new FakeReducer<int>(Incrementer);
            _store = new ReduxStore<int>(_incrementingReducer, stateFactory);
        }

        [Fact]
        public void WithoutMiddleware_DispatchOfSingleMessage_GivesExpectedState()
        {
            _store.Dispatch(new IncrementMessage(42));
            _store.State.Should().Be(42);
        }

        [Fact]
        public void WithMiddleware_DispatchOfSingleMessage_GivesExpectedState()
        {
            _store.AddMiddleware(_passThroughMiddleware);
            _store.Dispatch(new IncrementMessage(42));
            _store.State.Should().Be(42);
        }

        private int Incrementer(IReduxMessage message, int state) 
            => message is IncrementMessage inc ? state + inc.Increment : state;

        private class IncrementMessage : IReduxMessage
        {
            public int Increment { get; }

            public IncrementMessage(int increment)
                => Increment = increment;
        }
    }
}
