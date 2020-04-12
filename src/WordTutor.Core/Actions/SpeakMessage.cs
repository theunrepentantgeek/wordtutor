using System;
using System.Collections.Generic;
using System.Text;
using WordTutor.Core.Redux;

namespace WordTutor.Core.Actions
{
    public class SpeakMessage : IReduxMessage
    {
        public SpeakMessage(string textToSay)
            => TextToSay = textToSay;

        public string TextToSay { get; }

        public override string ToString() 
            => $"Speak '{TextToSay}'";
    }
}
