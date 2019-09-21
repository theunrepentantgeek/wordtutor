using WordTutor.Core.Redux;

namespace WordTutor.Core.Actions
{
    public class OpenScreenForModifyingWordMessage : IReduxMessage
    {
        public OpenScreenForModifyingWordMessage(VocabularyWord word)
        {
            Word = word;
        }

        public VocabularyWord Word { get; }
    }
}
