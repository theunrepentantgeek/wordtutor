using System;
using System.Collections.Generic;
using System.Text;

namespace WordTutor.Core.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Log the start of an action
        /// </summary>
        /// <remarks>
        /// Disposable so that the end of the action can be captured.
        /// </remarks>
        /// <param name="message">
        /// Description of the action to be performed.
        /// </param>
        IActionLogger Action(string message);

        /// <summary>
        /// Log info information about the application
        /// </summary>
        /// <param name="message">
        /// Information to be logged.
        /// </param>
        void Info(string message);

        /// <summary>
        /// Log debug information about the application
        /// </summary>
        /// <param name="message">
        /// Information to be logged.
        /// </param>
        void Debug(string message);
    }

    public interface IActionLogger : ILogger, IDisposable
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
