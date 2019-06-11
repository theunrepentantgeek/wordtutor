using System;

using FluentAssertions;
using Xunit;

namespace WordTutor.Core.Tests
{
    public class VocabularyBrowserScreenTests
    {
        public class Constructor : VocabularyBrowserScreenTests
        {
            [Fact]
            public void AfterConstruction_SelectionIsNull()
            {
                var browser = new VocabularyBrowserScreen();
                browser.Selection.Should().BeNull();
            }

            [Fact]
            public void AfterConstruction_ModifiedIsFalse()
            {
                var browser = new VocabularyBrowserScreen();
                browser.Modified.Should().BeFalse();
            }
        }

        public class WithSelection : VocabularyBrowserScreenTests
        {
            private readonly VocabularyBrowserScreen _browser;
            private readonly VocabularyWord _foo = new VocabularyWord("foo");
            private readonly VocabularyWord _bar = new VocabularyWord("bar");

            public WithSelection()
            {
                _browser = new VocabularyBrowserScreen()
                    .WithSelection(_foo);
            }

            [Fact]
            public void GivenNull_ThrowsExpectedException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _browser.WithSelection(null));
                exception.ParamName.Should().Be("word");
            }

            [Fact]
            public void GivenWord_ModifiesProperty()
            {
                var browser = _browser.WithSelection(_bar);
                browser.Selection.Should().Be(_bar);
            }

            [Fact]
            public void GivenExistingSelection_ReturnsSameInstance()
            {
                var browser = _browser.WithSelection(_browser.Selection);
                browser.Should().BeSameAs(_browser);
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ChangingSelection_DoesNotChangeModified(bool initiallyModified)
            {
                var initialBrowser = initiallyModified
                    ? _browser.MarkAsModified()
                    : _browser.MarkAsUnmodified();
                var browser = initialBrowser.WithSelection(_bar);
                browser.Modified.Should().Be(initialBrowser.Modified);
            }
        }

        public class MarkAsModified : VocabularyBrowserScreenTests
        {
            private readonly VocabularyBrowserScreen _browser = new VocabularyBrowserScreen();

            [Fact]
            public void SetsPropertyModified()
            {
                var browser = _browser.MarkAsModified();
                browser.Modified.Should().BeTrue();
            }
        }

        public class MarkAsUnmodified : VocabularyBrowserScreenTests
        {
            private readonly VocabularyBrowserScreen _browser =
                new VocabularyBrowserScreen().MarkAsModified();

            [Fact]
            public void ClearsPropertyModified()
            {
                var browser = _browser.MarkAsUnmodified();
                browser.Modified.Should().BeFalse();
            }
        }
    }
}
