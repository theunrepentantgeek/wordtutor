using WordTutor.Core.Actions;
using WordTutor.Core.Redux;

namespace WordTutor.Core.Reducers
{
    public class ModifyVocabularyWordScreenReducer : IReduxReducer<WordTutorApplication>
    {
        public WordTutorApplication Reduce(IReduxMessage message, WordTutorApplication currentState)
        {
            if (!(currentState.CurrentScreen is ModifyVocabularyWordScreen))
            {
                return currentState;
            }

            switch (message)
            {
                case ModifyPhraseMessage m:
                    return currentState.UpdateScreen(
                        (ModifyVocabularyWordScreen s) => s.WithPhrase(m.Phrase));

                case ModifyPronunciationMessage m:
                    return currentState.UpdateScreen(
                        (ModifyVocabularyWordScreen s) => s.WithPronunciation(m.Pronunciation));

                case ModifySpellingMessage m:
                    return currentState.UpdateScreen(
                        (ModifyVocabularyWordScreen s) => s.WithSpelling(m.Spelling));

                case SaveNewVocabularyWordMessage m:
                    return currentState.CloseScreen();
            }

            return currentState;
        }
    }
}
