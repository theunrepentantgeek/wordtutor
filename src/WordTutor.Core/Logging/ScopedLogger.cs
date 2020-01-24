using System;
using System.Diagnostics.CodeAnalysis;

namespace WordTutor.Core.Logging
{
    [SuppressMessage(
        "Usage",
        "CA1816:Dispose methods should call SuppressFinalize",
        Justification = "Dispose is being used for scope management, not resource disposal.")]
    [SuppressMessage(
        "Design",
        "CA1063:Implement IDisposable Correctly",
        Justification = "Dispose is being used for scope management, not resource disposal.")]
    public sealed class ScopedLogger : LoggerBase, IScopedLogger
    {
        private readonly DateTimeOffset _started = DateTimeOffset.Now;
        private readonly ScopedLogger? _parent;
        private bool _disposed;

        public ScopedLogger(ScopedLogger? parent)
        {
            _parent = parent;
            Level = parent is null ? 1 : parent.Level + 1;
        }

        public int Level { get; }

        public void Success(string message)
            => WriteLogMessage(LogKind.Success, message);

        public void Failure(string message)
            => WriteLogMessage(LogKind.Failure, message);

        public void Dispose()
        {
            var elapsed = DateTimeOffset.Now - _started;
            CurrentLogger = _parent;
            WriteLogMessage(LogKind.CloseAction, $"Elapsed {elapsed:c}.");
            _disposed = true;
        }

        protected override void WriteLogMessage(LogKind logKind, string message)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(
                    "Scoped loggers may not be used after disposal.");
            }

            base.WriteLogMessage(this, logKind, message);
        }
    }
}
