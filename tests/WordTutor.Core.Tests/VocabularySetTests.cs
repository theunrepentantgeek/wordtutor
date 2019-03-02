﻿using FluentAssertions;
using System;
using Xunit;

namespace WordTutor.Core.Tests
{
    public class VocabularySetTests
    {
        private readonly VocabularySet _set;
        private readonly string _name = "Sample Word Set";
        private readonly VocabularyWord _alpha = new VocabularyWord("alpha");
        private readonly VocabularyWord _beta = new VocabularyWord("beta");
        private readonly VocabularyWord _gamma = new VocabularyWord("gamma");
        private readonly VocabularyWord _epsilon = new VocabularyWord("epsilon");

        public VocabularySetTests()
        {
            _set = VocabularySet.Empty
                .WithName(_name)
                .Add(_alpha)
                .Add(_beta);
        }

        public class Empty : VocabularySetTests
        {
            [Fact]
            public void ReturnsSetWithNoName()
            {
                VocabularySet.Empty.Name.Should().BeEmpty();
            }

            [Fact]
            public void ReturnsSetWithNoWords()
            {
                VocabularySet.Empty.Words.Should().BeEmpty();
            }
        }

        public class WithName : VocabularySetTests
        {
            [Fact]
            public void GivenNull_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                    () => _set.WithName(null));
                exception.ParamName.Should().Be("name");
            }

            [Fact]
            public void GivenName_ReturnsSetWithNewName()
            {
                var newName = "Not the same";
                var set = _set.WithName(newName);
                set.Name.Should().Be(newName);
            }

            [Fact]
            public void GivenExistingName_ReturnsExistingSet()
            {
                var set = _set.WithName(_name);
                set.Should().BeSameAs(_set);
            }
        }

        public class Add : VocabularySetTests
        {
            private readonly VocabularySet _empty;

            public Add()
            {
                _empty = VocabularySet.Empty
                    .WithName(_name);
            }

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
                set.Words.Should().Contain(word);
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
                set.Words.Should().NotContain(_alpha);
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
                set.Words.Should().NotContain(_alpha);
            }

            [Fact]
            public void WhenReplacing_ReturnsVocabularySetWithReplacementWord()
            {
                var set = _set.Replace(_alpha, _gamma);
                set.Words.Should().Contain(_gamma);
            }

            [Fact]
            public void WhenReplacementEqualsExisting_ReturnsExistingSet()
            {
                var set = _set.Replace(_alpha, _alpha);
                set.Should().BeSameAs(_set);
            }
        }

        public class Update : VocabularySetTests
        {
            [Fact]
            public void GivenNullWord_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _set.Update(null, w => w.WithSpelling("alfa")));
                exception.ParamName.Should().Be("word");
            }

            [Fact]
            public void GivenNullTransform_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _set.Update("alpha", null));
                exception.ParamName.Should().Be("transform");
            }

            [Fact]
            public void GivenSpellingNotInSet_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentException>(
                        () => _set.Update("golf", w => w.WithSpelling("alfa")));
                exception.ParamName.Should().Be("word");
            }

            [Fact]
            public void WithValidTransform_CallsTransform()
            {
                var called = false;
                var set = _set.Update("alpha", Transform);
                called.Should().BeTrue();

                VocabularyWord Transform(VocabularyWord word)
                {
                    called = true;
                    return word;
                }
            }

            [Fact]
            public void WithValidTransform_RemovesOriginalWord()
            {
                var set = _set.Update("alpha", w => w.WithSpelling("alfa"));
                set.Words.Should().NotContain(w => w.HasSpelling("alpha"));
            }

            [Fact]
            public void WithValidTransform_AddsModifiedWord()
            {
                var set = _set.Update("alpha", w => w.WithSpelling("alfa"));
                set.Words.Should().Contain(w => w.HasSpelling("alfa"));
            }

            [Fact]
            public void WhenTransformReturnsOriginalWord_ReturnsOriginalSet()
            {
                var set = _set.Update("alpha", w => w);
                set.Should().BeSameAs(_set);
            }
        }
    }
}
