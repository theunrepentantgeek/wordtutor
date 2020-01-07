using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace WordTutor.Core.Logging
{
    public class Logger : ILogger
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
            var result = new ActionLogger(ActiveAction());
            UpdateActiveAction(result);
            return result;
        }

        public void Info(string message)
            => WriteLogMessage(LogKind.Info, message);

        public void Debug(string message)
            => WriteLogMessage(LogKind.Debug, message);

        protected void WriteLogMessage(LogKind logKind, string message)
        {
            var level = ActiveAction()?.Level ?? 0;
            var indent = new string(' ', level * 4);
            var now = DateTimeOffset.Now;
            System.Diagnostics.Debug.WriteLine($"{now:u} {indent}{_prefixes[logKind]} {message}");
        }

        protected virtual ActionLogger? ActiveAction()
            => _currentLogger.Value;

        protected static void UpdateActiveAction(ActionLogger? logger)
            => _currentLogger.Value = logger;

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

    public sealed class ActionLogger : Logger, IActionLogger
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

        [SuppressMessage(
            "Usage",
            "CA1816:Dispose methods should call SuppressFinalize",
            Justification = "Dispose is being used for scope management, not resource disposal.")]
        [SuppressMessage(
            "Design",
            "CA1063:Implement IDisposable Correctly",
            Justification = "Dispose is being used for scope management, not resource disposal.")]
        public void Dispose()
        {
            var elapsed = DateTimeOffset.Now - _started;
            UpdateActiveAction(_parent);
            WriteLogMessage(LogKind.CloseAction, $"Elapsed {elapsed:c}.");
            _disposed = true;
        }

        protected override ActionLogger? ActiveAction()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(
                    "Scoped loggers may not be used after disposal.");
            }

            return base.ActiveAction();
        }
    }
}
