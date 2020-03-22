using System;
using System.Collections.Generic;
using System.Text;

namespace WordTutor.Core
{
    /// <summary>
    /// A cache for a single value of <typeparamref name="T"/> that is expensive to compute.
    /// </summary>
    /// <remarks>
    /// Differs from <see cref="Lazy{T}"/> in being resettable - when the contained value is stale, 
    /// a call to <see cref="Clear"/> will remove the value, allowing a new value to be computed 
    /// on the next request.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public sealed class Cached<T>
        where T : class
    {
        private readonly Func<T> _generator;

        private readonly object _padlock = new object();

        private T? _value;

        private bool _valueKnown;

        public Cached(Func<T> generator)
        {
            _generator = generator ?? throw new ArgumentNullException(nameof(generator));
        }

        public void Clear()
        {
            lock (_padlock)
            {
                _value = null;
                _valueKnown = false;
            }
        }

        public T Value
        {
            get
            {
                lock(_padlock)
                {
                    if (!_valueKnown)
                    {
                        _value = _generator();
                        _valueKnown = true;
                    }

                    return _value!;
                }
            }
        }
    }
}
