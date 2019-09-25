using System;
using WordTutor.Core.Actions;
using WordTutor.Core.Redux;

namespace WordTutor.Core.Reducers
{
    public class ModifyVocabularyWordScreenReducer : IReduxReducer<WordTutorApplication>
    {
        public WordTutorApplication Reduce(IReduxMessage message, WordTutorApplication currentState)
        {
            if (currentState is null)
            {
                throw new ArgumentNullException(nameof(currentState));
            }

            if (!(currentState.CurrentScreen is ModifyVocabularyWordScreen))
            {
                return currentState;
            }

            return (message ?? throw new ArgumentNullException(nameof(message)))
                switch
                {
                    CloseScreenMessage m => currentState.CloseScreen(),

                    ModifyPhraseMessage m => currentState.UpdateScreen(
                        (ModifyVocabularyWordScreen s) => s.WithPhrase(m.Phrase)),

                    ModifyPronunciationMessage m => currentState.UpdateScreen(
                        (ModifyVocabularyWordScreen s) => s.WithPronunciation(m.Pronunciation)),

                    ModifySpellingMessage m => currentState.UpdateScreen(
                        (ModifyVocabularyWordScreen s) => s.WithSpelling(m.Spelling)),

                    SaveNewVocabularyWordMessage m => currentState.UpdateVocabularySet(
                        s => s.Add(m.Word))
                        .CloseScreen();

                    SaveModifiedVocabularyWordMessage m => currentState.UpdateVocabularySet(
                        s => s.Replace(m.OriginalWord, m.ReplacementWord))
                        .CloseScreen();

                    _ => currentState,
            };
        }
    }
}
