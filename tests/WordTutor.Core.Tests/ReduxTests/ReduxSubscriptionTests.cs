﻿using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using WordTutor.Core.Redux;
using Xunit;

namespace WordTutor.Core.Tests.ReduxTests
{
    public class ReduxSubscriptionTests
    {
        private int _whenCalledCount;
        private int _whenReleasedCount;
        private int _lastWhenCalledValue;
        private ReduxSubscription<string> _releasedSubscription;

        protected int Reader(string value) => value.Length;

        protected void WhenChanged(int value)
        {
            _lastWhenCalledValue = value;
            _whenCalledCount++;
        }

        protected void WhenReleased(ReduxSubscription<string> subscription)
        {
            _whenReleasedCount++;
            _releasedSubscription = subscription;
        }

        public class Constructor : ReduxSubscriptionTests
        {
            [Fact]
            public void GivenNullReader_ThrowsExpectedException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new ReduxSubscription<string, int>(null, WhenChanged, WhenReleased));
            }

            [Fact]
            public void GivenNullWhenChanged_ThrowsExpectedException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new ReduxSubscription<string, int>(Reader, null, WhenReleased));
            }

            [Fact]
            public void GivenNullWhenReleased_ThrowsExpectedException()
            {
                var exception =
                   Assert.Throws<ArgumentNullException>(
                       () => new ReduxSubscription<string, int>(Reader, WhenChanged, null));
            }
        }

        [SuppressMessage(
            "Design",
            "CA1001:Types that own disposable fields should be disposable",
            Justification = "Don't need to dispose tests.")]
        public class PublishTests : ReduxSubscriptionTests
        {
            private ReduxSubscription<string, int> _subscription;

            public PublishTests()
            {
                _subscription = new ReduxSubscription<string, int>(Reader, WhenChanged, WhenReleased);
            }

            [Fact]
            public void FirstPublication_InvokesWhenChangedWithValue()
            {
                var state = "sample";
                _subscription.Publish(state);
                _whenCalledCount.Should().Be(1);
                _lastWhenCalledValue.Should().Be(6);
            }

            [Fact]
            public void DuplicatePublication_DoesNotInvokeWhenChanged()
            {
                var state = "sample";
                _subscription.Publish(state);
                _subscription.Publish(state);
                _whenCalledCount.Should().Be(1);
                _lastWhenCalledValue.Should().Be(6);
            }

            [Fact]
            public void ChangedValue_DoesInvokeWhenChanged()
            {
                _subscription.Publish("sample");
                _subscription.Publish("demo");
                _whenCalledCount.Should().Be(2);
                _lastWhenCalledValue.Should().Be(4);
            }

            // Check that update loops will terminate/converge if the values are unchanged
            [Fact]
            public void CircularUpdates_DoNotLoopForever()
            {
                int loopCount = 0;
                ReduxSubscription<string, int> subscription = null;
                subscription = new ReduxSubscription<string, int>(
                    s => s.Length,
                    ValueChanged,
                    WhenReleased);
                subscription.Publish("bang");

                void ValueChanged(int value)
                {
                    loopCount++.Should().BeLessThan(10);
                    subscription.Publish("bang");
                }
            }
        }

        public class DisposalTests : ReduxSubscriptionTests
        {
            private ReduxSubscription<string, int> _subscription;

            public DisposalTests()
            {
                _subscription = new ReduxSubscription<string, int>(
                    Reader, WhenChanged, WhenReleased);
            }

            [Fact]
            public void AfterDisposal_PublishDoesNotInvokeWhenChanged()
            {
                _subscription.Dispose();
                _subscription.Publish("sample");
                _whenCalledCount.Should().Be(0);
            }

            [Fact]
            public void WhenDisposed_InvokesWhenReleased()
            {
                _subscription.Dispose();
                _whenReleasedCount.Should().Be(1);
            }

            [Fact]
            public void WhenDisposed_PassesSubscriptionToWhenReleased()
            {
                _subscription.Dispose();
                _releasedSubscription.Should().BeSameAs(_subscription);
            }

            [Fact]
            public void WhenDisposedAgain_DoesNotInvokeWhenReleased()
            {
                _subscription.Dispose();
                _subscription.Dispose();
                _whenReleasedCount.Should().Be(1);
            }
        }
    }
}