using WordTutor.Core.Redux;

namespace WordTutor.Core.Actions
{
    public class ModifySpellingMessage : IReduxMessage
    {
        public string Spelling { get; }

        public ModifySpellingMessage(string spelling)
        {
            Spelling = spelling;
        }

        public override string ToString()
            => $"Modify Spelling to '{Spelling}'";
    }
}
