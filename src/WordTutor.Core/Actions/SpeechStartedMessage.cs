using WordTutor.Core.Redux;

namespace WordTutor.Core.Actions
{
    public class SpeechStartedMessage :IReduxMessage
    {
        public SpeechStartedMessage(string speech)
            => Speech = speech;

        public string Speech { get; }

        public override string ToString()
            => $"Starting speech '{Speech}'";
    }

}
