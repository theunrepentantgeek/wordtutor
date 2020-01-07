using WordTutor.Core.Redux;

namespace WordTutor.Core.Actions
{
    public class ModifyPronunciationMessage : IReduxMessage
    {
        public string Pronunciation { get; }

        public ModifyPronunciationMessage(string pronunciation)
        {
            Pronunciation = pronunciation;
        }

        public override string ToString()
            => $"Modify Pronunciation to '{Pronunciation}'.";
    }
}
