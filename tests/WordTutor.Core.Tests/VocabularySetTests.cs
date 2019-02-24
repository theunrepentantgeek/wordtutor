using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace WordTutor.Core.Tests
{
    public class VocabularySetTests
    {
        private VocabularySet _set;
        private VocabularyWord _alpha = new VocabularyWord("alpha");
        private VocabularyWord _beta = new VocabularyWord("beta");
        private VocabularyWord _gamma = new VocabularyWord("gamma");
        private VocabularyWord _epsilon= new VocabularyWord("epsilon");

        public VocabularySetTests()
        {
            _set = VocabularySet.Empty
                .Add(_alpha)
                .Add(_beta);
        }

        public class Empty : VocabularySetTests
        {
            [Fact]
            public void ReturnsSetWithNoWords()
            {
                VocabularySet.Empty.Words.Should().BeEmpty();
            }
        }

        public class Add : VocabularySetTests
        {
            private VocabularySet _empty = VocabularySet.Empty;

            [Fact]
            public void GivenNull_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _empty.Add(null));
                exception.ParamName.Should().Be("word");
            }

            [Fact]
            public void GivenNewWord_ReturnsSetContainingWord()
            {
                var word = new VocabularyWord("bumble");
                var set = _empty.Add(word);
                set.Words.Should().ContainValue(word);
            }

            [Fact]
            public void GivenExistingWord_ThrowsException()
            {
                var word = new VocabularyWord("bumble");
                var set = _empty.Add(word);
                var exception = Assert.Throws<ArgumentException>(
                    () => set.Add(word));
                exception.ParamName.Should().Be("word");
                exception.Message.Should().Contain("already exists");

            }
        }

        public class Remove : VocabularySetTests
        {
            [Fact]
            public void GivenNull_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _set.Remove(null));
            }

            [Fact]
            public void GivenWordInSet_ReturnsNewSetWithoutWord()
            {
                var set = _set.Remove(_alpha);
                set.Words.Should().NotContainValue(_alpha);
            }

            [Fact]
            public void GivenWordNotInSet_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentException>(
                        () => _set.Remove(_gamma));
                exception.ParamName.Should().Be("word");
            }
        }

        public class Replace : VocabularySetTests
        {
            [Fact]
            public void GivenNullExistingWord_ThrowsException()
            {
                var exception =
                   Assert.Throws<ArgumentNullException>(
                       () => _set.Replace(null, _gamma));
                exception.ParamName.Should().Be("existing");
            }

            [Fact]
            public void GivenNullReplacementWord_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _set.Replace(_alpha, null));
                exception.ParamName.Should().Be("replacement");
            }

            [Fact]
            public void GivenExistingWordNotInSet_ThrowsException()
            {
                var exception =
                   Assert.Throws<ArgumentException>(
                       () => _set.Replace(_gamma, _epsilon));
                exception.ParamName.Should().Be("existing");
            }

            [Fact]
            public void WhenReplacing_ReturnsVocabularySetWithoutExistingWord()
            {
                var set = _set.Replace(_alpha, _gamma);
                set.Words.Should().NotContainValue(_alpha);
            }

            [Fact]
            public void WhenReplacing_ReturnsVocabularySetWithReplacementWord()
            {
                var set = _set.Replace(_alpha, _gamma);
                set.Words.Should().ContainValue(_gamma);
            }

            [Fact]
            public void WhenReplacementEqualsExisting_ReturnsExistingSet()
            {
                var set = _set.Replace(_alpha, _alpha);
                set.Should().BeSameAs(_set);
            }
        }
    }
}
