using FluentAssertions;
using System;
using WordTutor.Core.Redux;
using WordTutor.Core.Tests.Fakes;
using Xunit;

namespace WordTutor.Core.Tests.ReduxTests
{
    public class ReduxStoreTests
    {
        private readonly StringStateFactory _initialStateFactory = new StringStateFactory("alpha");
        private readonly FakeReducer<string> _reducer = new FakeReducer<string>();
        private readonly ReduxStore<string> _store;
        private readonly FakeMessage _message = new FakeMessage("message");

        public ReduxStoreTests()
        {
            _store = new ReduxStore<string>(_reducer, _initialStateFactory);
        }

        public class Constructor : ReduxStoreTests
        {
            [Fact]
            public void GivenNullReducer_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new ReduxStore<string>(null, _initialStateFactory));
                exception.ParamName.Should().Be("reducer");
            }

            [Fact]
            public void GivenInitialState_InitializesProperty()
            {

                var reducer = new FakeReducer<string>();
                var store = new ReduxStore<string>(reducer, _initialStateFactory);
                store.State.Should().Be(_initialStateFactory.State);
            }
        }

        public class Dispatch : ReduxStoreTests
        {
            [Fact]
            public void GivenNullMessage_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _store.Dispatch(null));
                exception.ParamName.Should().Be("message");
            }

            [Fact]
            public void GivenMessage_CallsReducerWithMessage()
            {
                FakeMessage receivedMessage = null;
                _reducer.Reduce = CaptureMessage;

                _store.Dispatch(_message);
                receivedMessage.Should().Be(_message);

                string CaptureMessage(IReduxMessage message, string state)
                {
                    receivedMessage = message as FakeMessage;
                    return state;
                }
            }

            [Fact]
            public void GivenMessage_CallsReducerWithState()
            {
                string initialState = _store.State;
                string receivedState = null;
                _reducer.Reduce = CaptureState;

                _store.Dispatch(_message);
                receivedState.Should().Be(initialState);

                string CaptureState(IReduxMessage message, string state)
                {
                    receivedState = state;
                    return state;
                }
            }

            [Fact]
            public void GivenMessage_UpdatesState()
            {
                var newState = "updated";
                _reducer.Reduce = ReduceToNewState;

                _store.Dispatch(_message);
                _store.State.Should().Be(newState);

                string ReduceToNewState(IReduxMessage message, string state)
                {
                    return newState;
                }
            }

            [Fact]
            public void RecursiveCall_ThrowsException()
            {
                var newState = "updated";
                _reducer.Reduce = RecursiveDispatch;

                var exception =
                    Assert.Throws<InvalidOperationException>(
                        () => _store.Dispatch(_message));
                exception.Message.Should().Contain("Dispatch");

                string RecursiveDispatch(IReduxMessage message, string state)
                {
                    _store.Dispatch(_message);
                    return newState;
                }
            }
        }

        private class StringStateFactory : IReduxStateFactory<string>
        {
            public StringStateFactory(string state) => State = state;

            public string State { get; }

            public string Create() => State;
        }
    }
}
