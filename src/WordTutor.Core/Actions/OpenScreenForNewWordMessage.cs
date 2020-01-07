using WordTutor.Core.Redux;

namespace WordTutor.Core.Actions
{
    public class OpenScreenForNewWordMessage : IReduxMessage
    {
        public override string ToString()
            => $"Open screen to create new word.";
    }
}
