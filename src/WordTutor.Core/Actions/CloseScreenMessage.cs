using WordTutor.Core.Redux;

namespace WordTutor.Core.Actions
{
    public class CloseScreenMessage : IReduxMessage
    {
        public override string ToString()
            => "Close screen.";
    }
}
