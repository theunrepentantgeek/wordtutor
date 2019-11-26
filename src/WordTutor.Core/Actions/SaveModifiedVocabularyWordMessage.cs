using System;
using WordTutor.Core.Redux;

namespace WordTutor.Core.Actions
{
    public class SaveModifiedVocabularyWordMessage : IReduxMessage
    {
        public VocabularyWord OriginalWord { get; }
        public VocabularyWord ReplacementWord { get; }

        public SaveModifiedVocabularyWordMessage(
            VocabularyWord originalWord, VocabularyWord replacementWord)
        {
            OriginalWord = originalWord 
                ?? throw new ArgumentNullException(nameof(originalWord));
            ReplacementWord = replacementWord 
                ?? throw new ArgumentNullException(nameof(replacementWord));
        }
    }
}
