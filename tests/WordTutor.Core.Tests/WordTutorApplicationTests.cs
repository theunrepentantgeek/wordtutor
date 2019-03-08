using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using WordTutor.Core.Tests.Fakes;
using Xunit;

namespace WordTutor.Core.Tests
{
    public class WordTutorApplicationTests
    {
        private readonly FakeScreen _fakeScreen = new FakeScreen();

        private readonly WordTutorApplication _app;

        public WordTutorApplicationTests()
        {
            _app = new WordTutorApplication(_fakeScreen);
        }

        public class Constructor : WordTutorApplicationTests
        {
            [Fact]
            public void GivenNullInitalScreen_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new WordTutorApplication(null));
                exception.ParamName.Should().Be("initialScreen");
            }

            [Fact]
            public void GivenInitialScreen_InitialisesProperty()
            {
                var screen = new FakeScreen();
                var app = new WordTutorApplication(screen);
                app.CurrentScreen.Should().BeSameAs(screen);
            }
        }
    }
}
