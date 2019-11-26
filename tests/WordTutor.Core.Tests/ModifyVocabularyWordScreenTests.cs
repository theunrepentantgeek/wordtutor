using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace WordTutor.Core.Tests
{
    public class ModifyVocabularyWordScreenTests
    {
        public class ForNewWord : ModifyVocabularyWordScreenTests
        {
            private readonly ModifyVocabularyWordScreen _screen
                = ModifyVocabularyWordScreen.ForNewWord();

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

            [Fact]
            public void CreatesScreenWithNoExistingWord()
            {
                _screen.ExistingWord.Should().BeNull();
            }

            [Fact]
            public void CreatesScreenThatIsUnmodified()
            {
                _screen.Modified.Should().BeFalse();
            }
        }

        public class ForExistingWord : ModifyVocabularyWordScreenTests
        {
            private readonly VocabularyWord _word
                = new VocabularyWord("wide")
                .WithPronunciation("wyde")
                .WithPhrase("That is a wide river.");

            private readonly ModifyVocabularyWordScreen _screen;

            public ForExistingWord()
            {
                _screen = ModifyVocabularyWordScreen.ForExistingWord(_word);
            }

            [Fact]
            public void CreatesScreenWithSpellingOfWord()
            {
                _screen.Spelling.Should().Be(_word.Spelling);
            }

            [Fact]
            public void CreatesScreenWithPhraseOfWord()
            {
                _screen.Phrase.Should().Be(_word.Phrase);
            }

            [Fact]
            public void CreatesScreenWithPronunciationOfWord()
            {
                _screen.Pronunciation.Should().Be(_word.Pronunciation);
            }

            [Fact]
            public void CreatesScreenWithExpectedExistingWord()
            {
                _screen.ExistingWord.Should().Be(_word);
            }

            [Fact]
            public void CreatesScreenThatIsUnmodified()
            {
                _screen.Modified.Should().BeFalse();
            }
        }

        public class WithSpelling : ModifyVocabularyWordScreenTests
        {
            private readonly ModifyVocabularyWordScreen _screen =
                ModifyVocabularyWordScreen.ForNewWord()
                    .WithSpelling("Foo")
                    .WithPhrase("Do the Foo")
                    .WithPronunciation("Fu")
                    .ClearModified();

            [Fact]
            public void GivenSpelling_UpdatesProperty()
            {
                const string s = "Bar";
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
                _screen.WithSpelling(null!).Spelling
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

            [Fact]
            public void GivenSpelling_SetsModified()
            {
                _screen.WithSpelling("Baz").Modified
                    .Should().BeTrue();
            }
        }

        public class WithPhrase : ModifyVocabularyWordScreenTests
        {
            private readonly ModifyVocabularyWordScreen _screen =
                ModifyVocabularyWordScreen.ForNewWord()
                    .WithSpelling("Foo")
                    .WithPhrase("Do the Foo")
                    .WithPronunciation("Fu");

            [Fact]
            public void GivenPhrase_UpdatesProperty()
            {
                const string s = "Do the Bar";
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
                _screen.WithPhrase(null!)
                    .Phrase.Should().Be(string.Empty);
            }

            [Fact]
            public void GivenPhrase_PreservesSpelling()
            {
                _screen.WithPhrase("Do the Baz")
                    .Spelling.Should().Be(_screen.Spelling);
            }

            [Fact]
            public void GivenPhrase_PreservesPronunciation()
            {
                _screen.WithPhrase("Do the Baz")
                    .Pronunciation.Should().Be(_screen.Pronunciation);
            }

            [Fact]
            public void GivenPhrase_SetsModified()
            {
                _screen.WithPhrase("Do the Baz").Modified
                    .Should().BeTrue();
            }
        }

        public class WithPronunciation : ModifyVocabularyWordScreenTests
        {
            private readonly ModifyVocabularyWordScreen _screen =
                ModifyVocabularyWordScreen.ForNewWord()
                    .WithSpelling("Foo")
                    .WithPhrase("Do the Foo")
                    .WithPronunciation("Fu");

            [Fact]
            public void GivenPronunciation_UpdatesProperty()
            {
                const string s = "Bu";
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
                _screen.WithPronunciation(null!)
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
                _screen.WithPronunciation("Bar")
                    .Phrase.Should().Be(_screen.Phrase);
            }

            [Fact]
            public void GivenPronounciation_SetsModified()
            {
                _screen.WithPhrase("Bar").Modified
                    .Should().BeTrue();
            }
        }

        public class AsWord : ModifyVocabularyWordScreenTests
        {
            private readonly VocabularyWord _word
                = new VocabularyWord("wide")
                .WithPronunciation("wyde")
                .WithPhrase("That is a wide river.");
            
            [Fact]
            public void WhenScreenConfigured_ReturnsExpectedWord()
            {
                var screen =
                    ModifyVocabularyWordScreen.ForNewWord()
                    .WithSpelling(_word.Spelling)
                    .WithPronunciation(_word.Pronunciation)
                    .WithPhrase(_word.Phrase);
                screen.AsWord().Should().Be(_word);
            }
        }
    }
}
