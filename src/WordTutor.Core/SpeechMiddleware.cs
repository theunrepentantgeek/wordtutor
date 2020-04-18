using System;
using System.Collections.Generic;
using System.Text;
using WordTutor.Core.Actions;
using WordTutor.Core.Redux;
using WordTutor.Core.Services;

namespace WordTutor.Core
{
    public class SpeechMiddleware : IReduxMiddleware
    {
        private readonly ISpeechService _speechServices;
        private readonly IReduxStore<WordTutorApplication> _reduxStore;

        public SpeechMiddleware(
            ISpeechService speechServices,
            IReduxStore<WordTutorApplication> reduxStore)
        {
            _speechServices = speechServices;
            _reduxStore = reduxStore;
        }

        public void Dispatch(IReduxMessage message, IReduxDispatcher next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (message is SpeakMessage speak)
            {
                Speak(speak.TextToSay);
            }

            next.Dispatch(message);
        }

        private async void Speak(string textToSay)
        {
            _reduxStore.Dispatch(new SpeechStartedMessage(textToSay));
            try
            {
                await _speechServices.SayAsync(textToSay).ConfigureAwait(false);
            }
            finally
            {
                _reduxStore.Dispatch(new SpeechFinishedMessage(textToSay));
            }
        }
    }
}
