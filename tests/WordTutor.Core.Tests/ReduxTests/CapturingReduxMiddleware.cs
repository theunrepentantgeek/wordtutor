using System;
using WordTutor.Core.Redux;

namespace WordTutor.Core.Tests.ReduxTests
{
    public class CapturingReduxMiddleware : IReduxMiddleware
    {
        public IReduxMessage? LastCapturedMessage { get; private set; }

        public void Dispatch(IReduxMessage message, IReduxDispatcher nextDispatcher)
        {
            if (nextDispatcher is null)
            {
                throw new ArgumentNullException(nameof(nextDispatcher));
            }

            LastCapturedMessage = message ?? throw new ArgumentNullException(nameof(message));
            nextDispatcher.Dispatch(message);
        }
    }
}
