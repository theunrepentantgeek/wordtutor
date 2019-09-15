using System;
using WordTutor.Core.Redux;

namespace WordTutor.Core.Actions
{
    public class SelectWordMessage : IReduxMessage
    {
        public VocabularyWord Word { get; }

        public SelectWordMessage(VocabularyWord word)
        {
            Word = word ?? throw new ArgumentNullException(nameof(word));
        }

        public override string ToString()
            => $"Select '{Word.Spelling}'";
    }
}
