using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WordTutor.Core.Redux
{
    [SuppressMessage(
        "Design",
        "CA1063:Implement IDisposable Correctly",
        Justification = "This class implements Dispose() for API design reasons.")]
    [SuppressMessage(
        "Usage",
        "CA1816:Dispose methods should call SuppressFinalize",
        Justification = "This class has no finalizer.")]
    public abstract class ReduxSubscription<TState> : IDisposable
    {
        private readonly Action<ReduxSubscription<TState>> _whenReleased;

        protected ReduxSubscription(Action<ReduxSubscription<TState>> whenReleased)
        {
            _whenReleased = whenReleased ?? throw new ArgumentNullException(nameof(whenReleased));
        }

        /// <summary>
        /// Gets a value indicating whether this subscription has been released
        /// </summary>
        public bool Released { get; private set; }

        /// <summary>
        /// Publish state to our subscriber
        /// </summary>
        /// <param name="state"></param>
        public abstract void Publish(TState state);

        public void Dispose() => Release();

        private void Release()
        {
            if (!Released)
            {
                Released = true;
                _whenReleased(this);
            }
        }
    }

    /// <summary>
    /// Represents a subscription to a redux store for state changes
    /// </summary>
    /// <typeparam name="TState">Type of value contained by the store.</typeparam>
    /// <typeparam name="TValue">Type of value subscribed.</typeparam>
    public sealed class ReduxReferenceSubscription<TState, TValue> : ReduxSubscription<TState>
        where TValue : class, IEquatable<TValue>?
    {
        private readonly Func<TState, TValue?> _reader;
        private readonly Action<TValue?> _whenChanged;

        private readonly EqualityComparer<TValue> _comparer = EqualityComparer<TValue>.Default;

        [AllowNull]
        private TValue _lastValue = default;

        private bool _haveEverPublished;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReduxSubscription{TState, TValue}"/> class
        /// </summary>
        /// <param name="reader">Function used read a value from our store state.</param>
        /// <param name="whenChanged">Action to invoke when the value changes.</param>
        /// <param name="whenReleased">Action to invoke when the subscription is disposed.</param>
        public ReduxReferenceSubscription(
            Func<TState, TValue?> reader,
            Action<TValue?> whenChanged,
            Action<ReduxSubscription<TState>> whenReleased)
            : base(whenReleased)
        {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _whenChanged = whenChanged ?? throw new ArgumentNullException(nameof(whenChanged));
        }

        /// <summary>
        /// Publish our state, invoking the callback if the value has changed
        /// </summary>
        /// <param name="state"></param>
        public override void Publish(TState state)
        {
            if (Released)
            {
                return;
            }

            var value = _reader(state);
            if (!_haveEverPublished || !_comparer.Equals(value, _lastValue))
            {
                _lastValue = value;
                _haveEverPublished = true;
                _whenChanged(value);
            }
        }
    }

    /// <summary>
    /// Represents a subscription to a redux store for state changes
    /// </summary>
    /// <typeparam name="TState">Type of value contained by the store.</typeparam>
    /// <typeparam name="TValue">Type of value subscribed.</typeparam>
    public sealed class ReduxValueSubscription<TState, TValue> : ReduxSubscription<TState>
        where TValue : struct, IEquatable<TValue>?
    {
        private readonly Func<TState, TValue> _reader;
        private readonly Action<TValue> _whenChanged;

        private readonly EqualityComparer<TValue> _comparer = EqualityComparer<TValue>.Default;

        [AllowNull]
        private TValue _lastValue = default;

        private bool _haveEverPublished;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReduxSubscription{TState, TValue}"/> class
        /// </summary>
        /// <param name="reader">Function used read a value from our store state.</param>
        /// <param name="whenChanged">Action to invoke when the value changes.</param>
        /// <param name="whenReleased">Action to invoke when the subscription is disposed.</param>
        public ReduxValueSubscription(
            Func<TState, TValue> reader,
            Action<TValue> whenChanged,
            Action<ReduxSubscription<TState>> whenReleased)
            : base(whenReleased)
        {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _whenChanged = whenChanged ?? throw new ArgumentNullException(nameof(whenChanged));
        }

        /// <summary>
        /// Publish our state, invoking the callback if the value has changed
        /// </summary>
        /// <param name="state"></param>
        public override void Publish(TState state)
        {
            if (Released)
            {
                return;
            }

            var value = _reader(state);
            if (!_haveEverPublished || !_comparer.Equals(value, _lastValue))
            {
                _lastValue = value;
                _haveEverPublished = true;
                _whenChanged(value);
            }
        }
    }
}
