using FluentAssertions;
using System;
using Xunit;

namespace WordTutor.Core.Tests
{
    public class VocabularyWordTests
    {
        public class Constructor : VocabularyWordTests
        {
            private string _spelling = "sample";

            [Fact]
            public void WithNullWord_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new VocabularyWord(null));
                exception.ParamName.Should().Be("spelling");
            }

            [Fact]
            public void WithWord_SetsSpellingProperty()
            {
                var word = new VocabularyWord(_spelling);
                word.Spelling.Should().Be(_spelling);
            }

            [Fact]
            public void SetsPronunciationToEmptyString()
            {
                var word = new VocabularyWord(_spelling);
                word.Pronunciation.Should().BeEmpty();
            }


            [Fact]
            public void SetsPhraseToEmptyString()
            {
                var word = new VocabularyWord(_spelling);
                word.Phrase.Should().BeEmpty();
            }
        }

        public class WithSpelling : VocabularyWordTests
        {
            private VocabularyWord _word = new VocabularyWord("sample")
                .WithPhrase("alpha")
                .WithPronunciation("beta");

            [Fact]
            public void WithNull_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _word.WithSpelling(null));
                exception.ParamName.Should().Be("spelling");
            }

            [Fact]
            public void WithNewSpelling_SetsProperty()
            {
                var newWord = _word.WithSpelling("bogus");
                newWord.Spelling.Should().Be("bogus");
            }

            [Fact]
            public void WithNewSpelling_LeavesOtherPropertiesUnchanged()
            {
                var newWord = _word.WithSpelling("bogus");
                newWord.Phrase.Should().Be(_word.Phrase);
                newWord.Pronunciation.Should().Be(_word.Pronunciation);
            }
        }

        public class WithPronunciation: VocabularyWordTests
        {
            private VocabularyWord _word = new VocabularyWord("sample");

            [Fact]
            public void WithNull_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _word.WithPronunciation(null));
                exception.ParamName.Should().Be("pronunciation");
            }

            [Fact]
            public void WithNewPronunciation_SetsProperty()
            {
                var newWord = _word.WithPronunciation("bugus");
                newWord.Pronunciation.Should().Be("bugus");
            }

            [Fact]
            public void WithNewPronunciation_LeavesOtherPropertiesUnchanged()
            {
                var newWord = _word.WithPronunciation("bogus");
                newWord.Spelling.Should().Be(_word.Spelling);
                newWord.Phrase.Should().Be(_word.Phrase);
            }
        }

        public class WithPhrase : VocabularyWordTests
        {
            private VocabularyWord _word = new VocabularyWord("sample");

            [Fact]
            public void WithNull_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _word.WithPhrase(null));
                exception.ParamName.Should().Be("phrase");
            }

            [Fact]
            public void WithNewPhrase_SetsProperty()
            {
                var newWord = _word.WithPhrase("That was such a bogus idea.");
                newWord.Phrase.Should().Be("That was such a bogus idea.");
            }

            [Fact]
            public void WithNewPhrase_LeavesOtherPropertiesUnchanged()
            {
                var newWord = _word.WithPhrase("bogus");
                newWord.Spelling.Should().Be(_word.Spelling);
                newWord.Pronunciation.Should().Be(_word.Pronunciation);
            }
        }

    }
}
