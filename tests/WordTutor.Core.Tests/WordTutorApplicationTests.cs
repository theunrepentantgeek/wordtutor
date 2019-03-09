﻿using FluentAssertions;
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

        public class OpenScreen : WordTutorApplicationTests
        {
            [Fact]
            public void GivenNullScreen_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _app.OpenScreen(null));
                exception.ParamName.Should().Be("screen");
            }

            [Fact]
            public void GivenScreen_MakesScreenCurrent()
            {
                var screen = new FakeScreen();
                var app = _app.OpenScreen(screen);
                app.CurrentScreen.Should().BeSameAs(screen);
            }
        }

        public class CloseScreen : WordTutorApplicationTests
        {
            [Fact]
            public void WhenCalled_RevealsPriorScreen()
            {
                var screen = new FakeScreen();
                var current = _app.CurrentScreen;
                var app = _app.OpenScreen(screen).CloseScreen();
                app.CurrentScreen.Should().Be(current);
            }

            [Fact]
            public void WhenCurrentScreenIsInitialScreen_ReturnsSameApplication()
            {
                var app = _app.CloseScreen();
                app.Should().BeSameAs(_app);
            }
        }

        public class UpdateScreen : WordTutorApplicationTests
        {
            [Fact]
            public void WhenNullTransformationSupplied_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _app.UpdateScreen(null));
                exception.ParamName.Should().Be("transformation");
            }

            [Fact]
            public void WhenTransformationReturnsScreen_UpdatesScreen()
            {
                var screen = new FakeScreen();
                var app = _app.UpdateScreen(s => screen);
                app.CurrentScreen.Should().Be(screen);
            }

            [Fact]
            public void WhenTransformationReturnsCurrentScreen_ReturnsSameInstance()
            {
                var app = _app.UpdateScreen(s => s);
                app.Should().BeSameAs(_app);
            }

            [Fact]
            public void WhenTransformationReturnsScreen_PriorScreenIsUnchanged()
            {
                var alpha = new FakeScreen();
                var beta = new FakeScreen();
                var current = _app.CurrentScreen;
                var app = _app.OpenScreen(alpha)
                    .UpdateScreen( s => beta)
                    .CloseScreen();
                app.CurrentScreen.Should().Be(current);
            }
        }
    }
}
