using System;
using WordTutor.Core.Redux;

namespace WordTutor.Core.Tests.ReduxTests
{
    /// <summary>
    /// Customizable middleware that allows different actions to be easily taken
    /// </summary>
    public class InterceptingReduxMiddleware : IReduxMiddleware
    {
        private readonly Action<IReduxMessage, IReduxDispatcher> _dispatch;

        public InterceptingReduxMiddleware(Action<IReduxMessage, IReduxDispatcher> dispatch)
        {
            _dispatch = dispatch ?? throw new ArgumentNullException(nameof(dispatch));
        }

        public void Dispatch(IReduxMessage message, IReduxDispatcher next)
            => Dispatch(message, next);
    }
}
