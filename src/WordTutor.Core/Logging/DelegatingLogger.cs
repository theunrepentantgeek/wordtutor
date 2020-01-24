namespace WordTutor.Core.Logging
{
    public sealed class DelegatingLogger : LoggerBase, ILogger
    {
        protected override void WriteLogMessage(LogKind logKind, string message)
            => WriteLogMessage(CurrentLogger, logKind, message);
    }
}
