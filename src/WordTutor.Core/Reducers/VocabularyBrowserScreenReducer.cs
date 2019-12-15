using System;
using WordTutor.Core.Actions;
using WordTutor.Core.Redux;

namespace WordTutor.Core.Reducers
{
    public class VocabularyBrowserScreenReducer : IReduxReducer<WordTutorApplication>
    {
        public WordTutorApplication Reduce(IReduxMessage message, WordTutorApplication currentState)
        {
            if (currentState is null)
            {
                throw new ArgumentNullException(nameof(currentState));
            }

            if (!(currentState.CurrentScreen is VocabularyBrowserScreen))
            {
                return currentState;
            }

            return (message ?? throw new ArgumentNullException(nameof(message)))
                switch
                {
                    ClearSelectedWordMessage _ => currentState.UpdateScreen(
                           (VocabularyBrowserScreen s) => s.ClearSelection()),

                    SelectWordMessage m => currentState.UpdateScreen(
                            (VocabularyBrowserScreen s) => s.WithSelection(m.Word)),

                    OpenScreenForNewWordMessage _ => currentState.OpenScreen(
					    ModifyVocabularyWordScreen.ForNewWord()),

                    OpenScreenForModifyingWordMessage m => currentState.OpenScreen(
						ModifyVocabularyWordScreen.ForExistingWord(m.Word)),

                    _ => currentState,
                };
        }
    }
}
