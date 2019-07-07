using System;
using WordTutor.Core.Redux;

namespace WordTutor.Core.Actions
{
    public class SaveNewVocabularyWordMessage : IReduxMessage
    {
        public VocabularyWord Word { get; }

        public SaveNewVocabularyWordMessage(VocabularyWord word)
        {
            Word = word ?? throw new ArgumentNullException(nameof(word));
        }
    }
}
