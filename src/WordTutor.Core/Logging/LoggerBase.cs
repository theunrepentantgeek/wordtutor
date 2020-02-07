using System;
using System.Collections.Generic;
using System.Threading;

using static System.Diagnostics.Debug;

namespace WordTutor.Core.Logging
{
    public abstract class LoggerBase
    {
        private static readonly AsyncLocal<ScopedLogger?> _currentLogger
            = new AsyncLocal<ScopedLogger?>();

        private readonly Dictionary<LogKind, string> _prefixes
            = new Dictionary<LogKind, string>
            {
                { LogKind.Debug, " · " },
                { LogKind.Info, "[•]" },
                { LogKind.OpenAction, "[>]" },
                { LogKind.CloseAction, "[<]" },
                { LogKind.Success, "[✔]" },
                { LogKind.Failure, "[✘]" }
            };

        private readonly List<string> _indents
            = new List<string> { string.Empty };

    public IScopedLogger Action(string message)
    {
        WriteLogMessage(LogKind.OpenAction, message);
        CurrentLogger = new ScopedLogger(CurrentLogger);
        return CurrentLogger;
    }

        public void Success(string message)
            => WriteLogMessage(LogKind.Success, message);

        public void Failure(string message)
            => WriteLogMessage(LogKind.Failure, message);

        public void Info(string message)
            => WriteLogMessage(LogKind.Info, message);

        public void Debug(string message)
            => WriteLogMessage(LogKind.Debug, message);

        protected abstract void WriteLogMessage(
            LogKind logKind,
            string message);

        protected void WriteLogMessage(ScopedLogger? currentAction, LogKind logKind, string message)
        {
            var level = currentAction?.Level ?? 0;
            var now = DateTimeOffset.Now;
            WriteLine($"{now:u} {Indent(level)}{_prefixes[logKind]} {message}");
        }

        protected static ScopedLogger? CurrentLogger
        {
            get => _currentLogger.Value;
            set => _currentLogger.Value = value;
        }

        private string Indent(int level)
        {
            while (_indents.Count <= level)
            {
                _indents.Add(new string(' ', _indents.Count * 4));
            }

            return _indents[level];
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
}
