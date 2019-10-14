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
                           (VocabularyBrowserScreen s) => s.WithNoSelection()),

                    SelectWordMessage m => currentState.UpdateScreen(
                            (VocabularyBrowserScreen s) => s.WithSelection(m.Word)),

                    OpenNewWordScreenMessage _ => currentState.OpenScreen(new ModifyVocabularyWordScreen()),

                    _ => currentState,
                };
        }
    }
}
