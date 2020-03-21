using FluentAssertions;
using System;
using WordTutor.Core.Redux;
using WordTutor.Core.Tests.Fakes;
using Xunit;

namespace WordTutor.Core.Tests.ReduxTests
{
    public class ReduxStoreTests
    {
        private readonly TestStateFactory<string> _initialStateFactory = new TestStateFactory<string>("alpha");
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
                        () => new ReduxStore<string>(null!, _initialStateFactory));
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
                        () => _store.Dispatch(null!));
                exception.ParamName.Should().Be("message");
            }

            [Fact]
            public void GivenMessage_CallsReducerWithMessage()
            {
                FakeMessage? receivedMessage = null;
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
                string receivedState = null!;
                _reducer.Reduce = CaptureState;

                _store.Dispatch(_message);
                receivedState.Should().Be(initialState);

                string CaptureState(IReduxMessage _, string state)
                {
                    receivedState = state;
                    return state;
                }
            }

            [Fact]
            public void GivenMessage_UpdatesState()
            {
                const string newState = "updated";
                _reducer.Reduce = ReduceToNewState;

                _store.Dispatch(_message);
                _store.State.Should().Be(newState);

                static string ReduceToNewState(IReduxMessage _, string __)
                {
                    return newState;
                }
            }

            [Fact]
            public void RecursiveCall_ThrowsException()
            {
                const string newState = "updated";
                _reducer.Reduce = RecursiveDispatch;

                var exception =
                    Assert.Throws<InvalidOperationException>(
                        () => _store.Dispatch(_message));
                exception.Message.Should().Contain("Dispatch");

                string RecursiveDispatch(IReduxMessage _, string __)
                {
                    _store.Dispatch(_message);
                    return newState;
                }
            }
        }

        public class Subscribe : ReduxStoreTests
        {
            private string? _handledValue;

            public Subscribe()
            {
                _reducer.Reduce = (m, s)
                    => m is FakeMessage f
                        ? f.Id
                        : s;
            }

            [Fact]
            public void GivenNullReader_ThrowsExpectedException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _store.SubscribeToReference<string>(null!, HandleUpdate));
                exception.ParamName.Should().Be("referenceReader");
            }

            [Fact]
            public void GivenNullAction_ThrowsExpectedException()
            {
                var exception =
                   Assert.Throws<ArgumentNullException>(
                       () => _store.SubscribeToReference<string>(ReadValue, null!));
                exception.ParamName.Should().Be("whenChanged");
            }

            [Fact]
            public void GivenValidParameters_ReturnsSubscription()
            {
                _store.SubscribeToReference(ReadValue, HandleUpdate).Should().NotBeNull();
            }

            [Fact]
            public void WhenSubscribed_ReceivesNotificationForChangesOfState()
            {
                var message = new FakeMessage("foo");
                using var subscription = _store.SubscribeToReference(ReadValue, HandleUpdate);
                _store.Dispatch(message);
                _handledValue.Should().Be(message.Id);
            }

            [Fact]
            public void WhenSubscribed_DoesNotReceiveNotificationIfValueUnchanged()
            {
                var message = new FakeMessage(_store.State);
                using (var subscription = _store.SubscribeToReference(ReadValue, HandleUpdate))
                {
                }

                _store.Dispatch(message);
                _handledValue.Should().BeNull();
            }

            [Fact]
            public void AfterSubscriptionReleased_SubscriptionCountIsReduced()
            {
                int subscriptionCount;
                using (var subscription = _store.SubscribeToReference(ReadValue, HandleUpdate))
                {
                    subscriptionCount = _store.SubscriptionCount;
                }

                _store.SubscriptionCount.Should().BeLessThan(subscriptionCount);
            }

            [Fact]
            public void AfterSubscriptionReleased_DoesNotReceiveNotification()
            {
                var message = new FakeMessage("foo");
                using (var subscription = _store.SubscribeToReference(ReadValue, HandleUpdate))
                {
                }

                _store.Dispatch(message);
                _handledValue.Should().BeNull();
            }

            private void HandleUpdate(string? value)
            {
                _handledValue = value;
            }

            private string ReadValue(string value) => value;
        }
    }
}
