using WordTutor.Core.Redux;

namespace WordTutor.Core.Tests.Fakes
{
    public class FakeMessage : IReduxMessage
    {
        public string Id { get; }

        public FakeMessage(string id)
        {
            Id = id;
        }
    }
}
