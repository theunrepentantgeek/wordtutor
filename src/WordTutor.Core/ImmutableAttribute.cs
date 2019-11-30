using System;
using System.Collections.Generic;
using System.Text;

namespace WordTutor.Core
{
    /// <summary>
    /// Marker attribute used to indicate that class instances are immutable
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ImmutableAttribute : Attribute
    {
    }
}
