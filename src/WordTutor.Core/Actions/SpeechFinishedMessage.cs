using WordTutor.Core.Redux;

namespace WordTutor.Core.Actions
{
    public class SpeechFinishedMessage : IReduxMessage
    {
        public SpeechFinishedMessage(string speech)
            => Speech = speech;

        public string Speech { get; }

        public override string ToString()
            => $"Finished speech '{Speech}'";
    }

}
