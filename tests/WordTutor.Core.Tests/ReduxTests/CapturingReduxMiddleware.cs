using WordTutor.Core.Redux;

namespace WordTutor.Core.Tests.ReduxTests
{
    public class CapturingReduxMiddleware : IReduxMiddleware
    {
        public IReduxMessage? LastCapturedMessage { get; private set; }

        public void Dispatch(IReduxMessage message, IReduxDispatcher next)
        {
            LastCapturedMessage = message;
            next.Dispatch(message);
        }
    }
}
