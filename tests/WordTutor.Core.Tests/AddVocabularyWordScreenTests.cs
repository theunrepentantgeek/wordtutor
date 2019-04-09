using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace WordTutor.Core.Tests
{
    public class AddVocabularyWordScreenTests
    {
        public class Constructor : AddVocabularyWordScreenTests
        {
            private readonly AddVocabularyWordScreen _screen = new AddVocabularyWordScreen();

            [Fact]
            public void CreatesScreenWithNoSpelling()
            {
                _screen.Spelling.Should().Be(string.Empty);
            }

            [Fact]
            public void CreatesScreenWithNoPhrase()
            {
                _screen.Phrase.Should().Be(string.Empty);
            }

            [Fact]
            public void CreatesScreenWithNoPronunciation()
            {
                _screen.Pronunciation.Should().Be(string.Empty);
            }
        }

        public class WithSpelling : AddVocabularyWordScreenTests
        {
            private readonly AddVocabularyWordScreen _screen =
                new AddVocabularyWordScreen()
                    .WithSpelling("Foo")
                    .WithPhrase("Do the Foo")
                    .WithPronunciation("Fu");

            [Fact]
            public void GivenSpelling_UpdatesProperty()
            {
                var s = "Bar";
                _screen.WithSpelling(s).Spelling.Should().Be(s);
            }

            [Fact]
            public void GivenEmptySpelling_UpdatesProperty()
            {
                _screen.WithSpelling(string.Empty).Spelling
                    .Should().Be(string.Empty);
            }

            [Fact]
            public void GivenNullSpelling_UpdatesPropertyToEmpty()
            {
                _screen.WithSpelling(null).Spelling
                    .Should().Be(string.Empty);
            }

            [Fact]
            public void GivenSpelling_PreservesPhrase()
            {
                _screen.WithSpelling("Baz").Phrase
                    .Should().Be(_screen.Phrase);
            }

            [Fact]
            public void GivenSpelling_PreservesPronunciation()
            {
                _screen.WithSpelling("Baz").Pronunciation
                    .Should().Be(_screen.Pronunciation);
            }
        }

        public class WithPhrase : AddVocabularyWordScreenTests
        {
            private readonly AddVocabularyWordScreen _screen =
                new AddVocabularyWordScreen()
                    .WithSpelling("Foo")
                    .WithPhrase("Do the Foo")
                    .WithPronunciation("Fu");

            [Fact]
            public void GivenPhrase_UpdatesProperty()
            {
                var s = "Do the Bar";
                _screen.WithPhrase(s).Phrase.Should().Be(s);
            }

            [Fact]
            public void GivenEmptyPhrase_UpdatesProperty()
            {
                _screen.WithPhrase(string.Empty)
                    .Phrase.Should().Be(string.Empty);
            }

            [Fact]
            public void GivenNullPhrase_UpdatesPropertyToEmpty()
            {
                _screen.WithPhrase(null)
                    .Phrase.Should().Be(string.Empty);
            }

            [Fact]
            public void GivenPhrase_PreservesSpelling()
            {
                _screen.WithPhrase("Do the Baz")
                    .Spelling.Should().Be(_screen.Spelling);
            }

            [Fact]
            public void GivenSpelling_PreservesPronunciation()
            {
                _screen.WithPhrase("Do the Baz")
                    .Pronunciation.Should().Be(_screen.Pronunciation);
            }
        }

        public class WithPronunciation : AddVocabularyWordScreenTests
        {
            private readonly AddVocabularyWordScreen _screen =
                new AddVocabularyWordScreen()
                    .WithSpelling("Foo")
                    .WithPhrase("Do the Foo")
                    .WithPronunciation("Fu");

            [Fact]
            public void GivenPronunciation_UpdatesProperty()
            {
                var s = "Bu";
                _screen.WithPronunciation(s).Pronunciation.Should().Be(s);
            }

            [Fact]
            public void GivenEmptyPronunciation_UpdatesProperty()
            {
                _screen.WithPronunciation(string.Empty)
                    .Pronunciation.Should().Be(string.Empty);
            }

            [Fact]
            public void GivenNullPronunciation_UpdatesPropertyToEmpty()
            {
                _screen.WithPronunciation(null)
                    .Pronunciation.Should().Be(string.Empty);
            }

            [Fact]
            public void GivenPronunciation_PreservesSpelling()
            {
                _screen.WithPronunciation("Bar")
                    .Spelling.Should().Be(_screen.Spelling);
            }

            [Fact]
            public void GivenPronunciation_PreservesPhrase()
            {
                _screen.WithPronunciation("Do the Baz")
                    .Phrase.Should().Be(_screen.Phrase);
            }
        }
    }
}
