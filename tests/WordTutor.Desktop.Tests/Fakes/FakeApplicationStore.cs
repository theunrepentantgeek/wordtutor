using System;
using System.Collections.Generic;
using System.Text;
using WordTutor.Core;
using WordTutor.Core.Redux;

namespace WordTutor.Desktop.Tests.Fakes
{
    public class FakeApplicationStore : IReduxStore<WordTutorApplication>
    {
        public WordTutorApplication State { get; }

        public IReduxMessage Message { get; private set; }

        public FakeApplicationStore(WordTutorApplication application)
        {
            State = application;
        }

        public void Dispatch(IReduxMessage message) => Message = message;
    }
}
