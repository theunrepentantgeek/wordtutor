using WordTutor.Core.Redux;

namespace WordTutor.Core.Actions
{
    public class SelectWordMessage : IReduxMessage
    {
        public VocabularyWord Word { get; }

        public SelectWordMessage(VocabularyWord word)
        {
            Word = word;
        }

        public override string ToString()
            => $"Select '{Word.Spelling}'";
    }
}
