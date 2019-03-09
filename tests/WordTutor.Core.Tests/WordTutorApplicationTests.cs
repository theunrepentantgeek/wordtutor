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
                        () => new WordTutorApplication(null!));
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
                        () => _app.OpenScreen(null!));
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
                        () => _app.UpdateScreen<FakeScreen, FakeScreen>(null!));
                exception.ParamName.Should().Be("transformation");
            }

            [Fact]
            public void WhenTransformationReturnsScreen_UpdatesScreen()
            {
                var screen = new FakeScreen();
                var app = _app.UpdateScreen((FakeScreen _) => screen);
                app.CurrentScreen.Should().Be(screen);
            }

            [Fact]
            public void WhenTransformationReturnsCurrentScreen_ReturnsSameInstance()
            {
                var app = _app.UpdateScreen((FakeScreen s) => s);
                app.Should().BeSameAs(_app);
            }

            [Fact]
            public void WhenTransformationReturnsScreen_PriorScreenIsUnchanged()
            {
                var alpha = new FakeScreen();
                var beta = new FakeScreen();
                var current = _app.CurrentScreen;
                var app = _app.OpenScreen(alpha)
                    .UpdateScreen((FakeScreen _) => beta)
                    .CloseScreen();
                app.CurrentScreen.Should().Be(current);
            }
        }

        public class WithVocabularySet : WordTutorApplicationTests
        {
            private readonly VocabularySet _set;

            public WithVocabularySet()
            {
                var alpha = new VocabularyWord("alpha");
                var beta = new VocabularyWord("beta");
                _set = VocabularySet.Empty
                    .Add(alpha)
                    .Add(beta);
            }

            [Fact]
            public void GivenNullVocabularySet_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _app.WithVocabularySet(null!));
                exception.ParamName.Should().Be("vocabularySet");
            }

            [Fact]
            public void GivenVocabularySet_UpdatesProperty()
            {
                var app = _app.WithVocabularySet(_set);
                app.VocabularySet.Should().BeSameAs(_set);
            }

            [Fact]
            public void GivenExistingVocabularySet_ReturnsExistingApplication()
            {
                var app = _app.WithVocabularySet(_app.VocabularySet);
                app.Should().BeSameAs(_app);
            }
        }

        public class UpdateVocabularySet : WordTutorApplicationTests
        {
            [Fact]
            public void GivenNull_ThrowsException()
            {
                var exception =
                 Assert.Throws<ArgumentNullException>(
                     () => _app.UpdateVocabularySet(null!));
                exception.ParamName.Should().Be("transformation");
            }

            [Fact]
            public void GivenTransformation_ItReceivesCurrentVocabularySet()
            {
                VocabularySet? set = null;
                var app = _app.UpdateVocabularySet(
                    s =>
                    {
                        set = s;
                        return s;
                    });
                set.Should().Be(_app.VocabularySet);
            }

            [Fact]
            public void GivenTransformation_UpdatesPropertyToReturnedValue()
            {
                var alpha = new VocabularyWord("alpha");
                var set = _app.VocabularySet.Add(alpha);
                var app = _app.UpdateVocabularySet(_ => set);
                app.VocabularySet.Should().Be(set);
            }

            [Fact]
            public void WhenTransformationReturnsExistingSet_ReturnsExistingApp()
            {
                var app = _app.UpdateVocabularySet(s => s);
                app.Should().BeSameAs(_app);
            }
        }
    }
}
