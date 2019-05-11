using System.Diagnostics.CodeAnalysis;

namespace WordTutor.Core.Redux
{
    /// <summary>
    /// Marker interface for messages that transform the state of the application
    /// </summary>
    /// <remarks>
    /// Messages should typically be simple and immutable.
    /// </remarks>
    [SuppressMessage(
        "Design",
        "CA1040:Avoid empty interfaces",
        Justification = "Marker interface used to prevent other types being used as Redux messages")]
    public interface IReduxMessage
    {
    }
}
