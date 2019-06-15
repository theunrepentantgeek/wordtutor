using System;
using System.Collections.Generic;
using System.Text;
using WordTutor.Core.Redux;

namespace WordTutor.Core
{
    public class WordTutorAppicationReducer : IReduxReducer<WordTutorApplication>
    {
        private readonly IReduxReducer<Screen> _reduxScreen;

        public WordTutorAppicationReducer(IReduxReducer<Screen> reduxScreen)
        {
            _reduxScreen = reduxScreen;
        }

        public WordTutorApplication Reduce(IReduxMessage message, WordTutorApplication currentState)
        {
            return currentState.UpdateScreen(
                screen => _reduxScreen.Reduce(message, screen));
        }
    }
}
