using WordTutor.Core.Redux;

namespace WordTutor.Core.Tests.ReduxTests
{
    public class BlockingReduxMiddleware : IReduxMiddleware
    {
        public void Dispatch(IReduxMessage message, IReduxDispatcher next)
        {
            // do nothing
        }
    }
}
