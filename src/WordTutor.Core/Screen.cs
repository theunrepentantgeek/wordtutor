using System;

namespace WordTutor.Core
{
    [Immutable]
    public abstract class Screen : IEquatable<Screen?>
    {
        public abstract bool Equals(Screen? other);

        public override bool Equals(object? obj)
            => Equals(obj as Screen);

        public abstract override int GetHashCode();
    }
}
