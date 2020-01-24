using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace WordTutor.Core.Logging
{
    public abstract class LoggerBase : ILogger
    {
        private static readonly AsyncLocal<ActionLogger?> _currentLogger
            = new AsyncLocal<ActionLogger?>();

        private readonly Dictionary<LogKind, string> _prefixes
            = new Dictionary<LogKind, string>
            {
                {LogKind.Debug, " · " },
                {LogKind.Info, "[•]" },
                {LogKind.OpenAction, "[>]" },
                {LogKind.CloseAction, "[<]" },
                {LogKind.Success, "[✔]" },
                {LogKind.Failure, "[✘]" }
            };

        public IActionLogger Action(string message)
        {
            WriteLogMessage(LogKind.OpenAction, message);
            CurrentLogger = new ActionLogger(CurrentLogger);
            return CurrentLogger;
        }

        public void Info(string message)
            => WriteLogMessage(LogKind.Info, message);

        public void Debug(string message)
            => WriteLogMessage(LogKind.Debug, message);

        protected abstract void WriteLogMessage(LogKind logKind, string message);

        protected void WriteLogMessage(ActionLogger? currentAction, LogKind logKind, string message)
        {
            var level = currentAction?.Level ?? 0;
            var indent = new string(' ', level * 4);
            var now = DateTimeOffset.Now;
            System.Diagnostics.Debug.WriteLine($"{now:u} {indent}{_prefixes[logKind]} {message}");
        }

        protected static ActionLogger? CurrentLogger
        {
            get => _currentLogger.Value;
            set => _currentLogger.Value = value;
        }

        protected enum LogKind
        {
            None,
            Debug,
            Info,
            OpenAction,
            CloseAction,
            Success,
            Failure
        }
    }

    public sealed class DelegatingLogger : LoggerBase, ILogger
    {
        protected override void WriteLogMessage(LogKind logKind, string message)
            => WriteLogMessage(CurrentLogger, logKind, message);
    }

    [SuppressMessage(
        "Usage",
        "CA1816:Dispose methods should call SuppressFinalize",
        Justification = "Dispose is being used for scope management, not resource disposal.")]
    [SuppressMessage(
        "Design",
        "CA1063:Implement IDisposable Correctly",
        Justification = "Dispose is being used for scope management, not resource disposal.")]
    public sealed class ActionLogger : LoggerBase, IActionLogger
    {
        private readonly DateTimeOffset _started = DateTimeOffset.Now;
        private readonly ActionLogger? _parent;
        private bool _disposed;

        public ActionLogger(ActionLogger? parent)
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
