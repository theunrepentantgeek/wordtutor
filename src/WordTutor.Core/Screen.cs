using System;

namespace WordTutor.Core
{
    public abstract class Screen : IEquatable<Screen>
    {
        public abstract bool Equals(Screen other);
    }

}
