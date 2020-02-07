namespace WordTutor.Core.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Log the start of an action
        /// </summary>
        /// <remarks>
        /// Returns a disposable IScopedLogger so that the end of the action can be captured.
        /// </remarks>
        /// <param name="message">
        /// Description of the action to be performed.
        /// </param>
        IScopedLogger Action(string message);

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
}
