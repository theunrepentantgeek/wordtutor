using WordTutor.Core.Actions;
using WordTutor.Core.Redux;

namespace WordTutor.Core.Reducers
{
    public class VocabularyBrowserScreenReducer : IReduxReducer<WordTutorApplication>
    {
        public WordTutorApplication Reduce(IReduxMessage message, WordTutorApplication currentState)
        {
            if (!(currentState.CurrentScreen is VocabularyBrowserScreen))
            {
                return currentState;
            }

            switch(message)
            {
                case ClearSelectedWordMessage _:
                    return currentState.UpdateScreen(
                        (VocabularyBrowserScreen s) => s.WithNoSelection());
                case SelectWordMessage m:
                    return currentState.UpdateScreen(
                        (VocabularyBrowserScreen s) => s.WithSelection(m.Word));
                case OpenNewWordScreenMessage _:
                    return currentState.OpenScreen(new AddVocabularyWordScreen());
            }

            return currentState;
        }
    }
}
