using WordTutor.Core.Redux;

namespace WordTutor.Core.Actions
{
    public class ClearSelectedWordMessage : IReduxMessage
    {
        public override string ToString()
            => "Clear selection.";
    }
}
