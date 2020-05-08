using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using WordTutor.Core.Redux;
using WordTutor.Core.Tests.Fakes;
using Xunit;

namespace WordTutor.Core.Tests.ReduxTests
{
    public class ReduxStoreTests
    {
        private readonly TestStateFactory<string> _initialStateFactory = new TestStateFactory<string>("alpha");
        private readonly FakeMessage _message = new FakeMessage("message");
        private readonly Lazy<ReduxStore<string>> _store;

        private FakeReducer<string>? _reducer;

        public ReduxStoreTests()
        {
            _store = new Lazy<ReduxStore<string>>(CreateStore);
        }

        protected ReduxStore<string> Store => _store.Value;

        [SuppressMessage(
            "Globalization", 
            "CA1303:Do not pass literals as localized parameters", 
            Justification = "This project does not localize exception messages.")]
        private ReduxStore<string> CreateStore()
            => new ReduxStore<string>(
                _reducer ?? throw new InvalidOperationException("_reducer has not been initialized"),
                _initialStateFactory);

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
                _reducer = new FakeReducer<string>(IgnoreMessages);
                Store.State.Should().Be(_initialStateFactory.State);
            }

            private string IgnoreMessages(IReduxMessage message, string state)
                => state;
        }

        public class Dispatch : ReduxStoreTests
        {
            [Fact]
            public void GivenNullMessage_ThrowsException()
            {
                _reducer = new FakeReducer<string>((_, s) => s);
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => Store.Dispatch(null!));
                exception.ParamName.Should().Be("message");
            }

            [Fact]
            public void GivenMessage_CallsReducerWithMessage()
            {
                FakeMessage? receivedMessage = null;
                _reducer = new FakeReducer<string>(CaptureMessage);

                Store.Dispatch(_message);
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
                _reducer = new FakeReducer<string>(CaptureState);
                string initialState = Store.State;
                string receivedState = null!;

                Store.Dispatch(_message);
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
                _reducer = new FakeReducer<string>(ReduceToNewState);

                Store.Dispatch(_message);
                Store.State.Should().Be(newState);

                static string ReduceToNewState(IReduxMessage _, string __)
                {
                    return newState;
                }
            }

            [Fact(Skip ="Test is yet to be written")]
            public void RecursiveCall_DispatchesMessagesInSequence()
            {
                // TODO
            }
        }

        public class Subscribe : ReduxStoreTests
        {
            private string? _handledValue;

            public Subscribe()
            {
                _reducer = new FakeReducer<string>(
                    (m, s) => m is FakeMessage f ? f.Id : s);
            }

            [Fact]
            public void GivenNullReader_ThrowsExpectedException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => Store.SubscribeToReference<string>(null!, HandleUpdate));
                exception.ParamName.Should().Be("referenceReader");
            }

            [Fact]
            public void GivenNullAction_ThrowsExpectedException()
            {
                var exception =
                   Assert.Throws<ArgumentNullException>(
                       () => Store.SubscribeToReference<string>(ReadValue, null!));
                exception.ParamName.Should().Be("whenChanged");
            }

            [Fact]
            public void GivenValidParameters_ReturnsSubscription()
            {
                Store.SubscribeToReference(ReadValue, HandleUpdate).Should().NotBeNull();
            }

            [Fact]
            public void WhenSubscribed_ReceivesNotificationForChangesOfState()
            {
                var message = new FakeMessage("foo");
                using var subscription = Store.SubscribeToReference(ReadValue, HandleUpdate);
                Store.Dispatch(message);
                _handledValue.Should().Be(message.Id);
            }

            [Fact]
            public void WhenSubscribed_DoesNotReceiveNotificationIfValueUnchanged()
            {
                var message = new FakeMessage(Store.State);
                using (var subscription = Store.SubscribeToReference(ReadValue, HandleUpdate))
                {
                }

                Store.Dispatch(message);
                _handledValue.Should().BeNull();
            }

            [Fact]
            public void AfterSubscriptionReleased_SubscriptionCountIsReduced()
            {
                int subscriptionCount;
                using (var subscription = Store.SubscribeToReference(ReadValue, HandleUpdate))
                {
                    subscriptionCount = Store.SubscriptionCount;
                }

                Store.SubscriptionCount.Should().BeLessThan(subscriptionCount);
            }

            [Fact]
            public void AfterSubscriptionReleased_DoesNotReceiveNotification()
            {
                var message = new FakeMessage("foo");
                using (var subscription = Store.SubscribeToReference(ReadValue, HandleUpdate))
                {
                }

                Store.Dispatch(message);
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
