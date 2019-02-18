using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace WordTutor.Core.Tests
{
    public class VocabularySetTests
    {
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
    }
}
