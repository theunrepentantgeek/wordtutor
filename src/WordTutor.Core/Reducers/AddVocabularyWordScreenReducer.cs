using WordTutor.Core.Actions;
using WordTutor.Core.Redux;

namespace WordTutor.Core.Reducers
{
    public class AddVocabularyWordScreenReducer : IReduxReducer<WordTutorApplication>
    {
        public WordTutorApplication Reduce(IReduxMessage message, WordTutorApplication currentState)
        {
            if (!(currentState.CurrentScreen is AddVocabularyWordScreen))
            {
                return currentState;
            }

            switch (message)
            {
                case ModifySpellingMessage m:
                    return currentState.UpdateScreen(
                        (AddVocabularyWordScreen s) => s.WithSpelling(m.Spelling));

                case ModifyPhraseMessage m:
                    return currentState.UpdateScreen(
                        (AddVocabularyWordScreen s) => s.WithPhrase(m.Phrase));

                case ModifyPronunciationMessage m:
                    return currentState.UpdateScreen(
                        (AddVocabularyWordScreen s) => s.WithPronunciation(m.Pronunciation));
            }

            return currentState;
        }
    }
}
