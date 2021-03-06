﻿using System;
using System.Collections.Generic;
using System.Text;
using WordTutor.Core.Logging;

namespace WordTutor.Desktop.Tests.Fakes
{
    public sealed class FakeLogger : ILogger, IScopedLogger
    {
        public IScopedLogger Action(string message)
            => this;

        public void Debug(string message)
        {
        }

        public void Dispose()
        {
        }

        public void Failure(string message)
        {
        }

        public void Info(string message)
        {
        }

        public void Success(string message)
        {
        }
    }
}
