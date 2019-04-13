using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using WordTutor.Core;
using WordTutor.Core.Redux;

namespace WordTutor.Desktop.Tests.Fakes
{
    public class FakeApplicationStore : IReduxStore<WordTutorApplication>
    {
        private readonly List<IReduxMessage> _messages =
            new List<IReduxMessage>();

        public WordTutorApplication State { get; }

        public FakeApplicationStore(WordTutorApplication application)
        {
            State = application;
        }

        public void Dispatch(IReduxMessage message) =>
            _messages.Add(message);

        [SuppressMessage(
            "Naming",
            "CA1715:Identifiers should have correct prefix",
            Justification = "Prefer to use 'M' instead of 'T' since our type represents a message")]
        public M AssertReceived<M>()
            where M : IReduxMessage
        {
            var result = _messages.OfType<M>()
                .FirstOrDefault();
            result.Should().NotBeNull(
                $"expect to have received a message of type '{typeof(M).Name}'");
            return result;
        }

        [SuppressMessage(
            "Naming", 
            "CA1715:Identifiers should have correct prefix", 
            Justification = "Prefer to use 'M' instead of 'T' since our type represents a message")]
        public void AssertDidNotReceive<M>()
            where M : IReduxMessage
        {
            var result = _messages.OfType<M>()
                .FirstOrDefault();
            result.Should().BeNull(
                $"did not expect to have received a message of type '{typeof(M).Name}'");
        }
    }
}
