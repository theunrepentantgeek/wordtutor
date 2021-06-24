using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using WordTutor.Core.Logging;
using WordTutor.Core.Services;
using WordTutor.Desktop.Tests.Fakes;
using Xunit;

namespace WordTutor.Desktop.Tests
{
    public class SpeechServiceTests
    {
        public class Constructor : SpeechServiceTests
        {
            private readonly IRenderSpeechService _render = new FakeRenderSpeechService();
            private readonly ILogger _logger = new FakeLogger();

            [Fact]
            public void GivenNullRenderSpeechService_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new SpeechService(null!, _logger));
                exception.ParamName.Should().Be("renderSpeechService");
            }

            [Fact]
            public void GivenNullLogger_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new SpeechService(_render, null!));
                exception.ParamName.Should().Be("renderSpeechService");
            }
        }
    }
}
