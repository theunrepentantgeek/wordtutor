using WordTutor.Core.Redux;

namespace WordTutor.Core.Actions
{
    public class ModifyPhraseMessage : IReduxMessage
    {
        public string Phrase { get; }

        public ModifyPhraseMessage(string phrase)
        {
            Phrase = phrase;
        }
    }
}
