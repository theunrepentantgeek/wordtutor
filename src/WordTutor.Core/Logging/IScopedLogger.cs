using System;

namespace WordTutor.Core.Logging
{
    public interface IScopedLogger : ILogger, IDisposable
    {
        /// <summary>
        /// Indicate that the current action (or part thereof) has been successful
        /// </summary>
        /// <param name="message">
        /// Information about the successful outcome.
        /// </param>
        void Success(string message);

        /// <summary>
        /// Indicate that the current action (or part thereof) has failed
        /// </summary>
        /// <param name="message">
        /// Information about the failure
        /// </param>
        void Failure(string message);
    }
}
