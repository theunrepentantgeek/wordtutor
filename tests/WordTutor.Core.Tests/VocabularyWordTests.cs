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

        public class EqualsVocabularyWord : VocabularyWordTests
        {
            private readonly string _spelling = "bogus";
            private readonly string _phrase = "this is a bogus word.";
            private readonly string _pronunciation = "boogus";

            private readonly VocabularyWord _word;

            public EqualsVocabularyWord()
            {
                _word = new VocabularyWord(_spelling)
                    .WithPhrase(_phrase)
                    .WithPronunciation(_pronunciation);
            }

            [Fact]
            public void GivenNull_ReturnsFalse()
            {
                VocabularyWord other = null;
                _word.Equals(other).Should().BeFalse();
            }

            [Fact]
            public void GivenSelf_ReturnsTrue()
            {
                var other = _word;
                _word.Equals(other).Should().BeTrue();
            }

            [Fact]
            public void GivenIdentical_ReturnsTrue()
            {
                var other = new VocabularyWord(_spelling)
                    .WithPhrase(_phrase)
                    .WithPronunciation(_pronunciation);
                _word.Equals(other).Should().BeTrue();
            }

            [Fact]
            public void GivenDifferentSpelling_ReturnsFalse()
            {
                var other = _word.WithSpelling("other");
                _word.Equals(other).Should().BeFalse();
            }

            [Fact]
            public void GivenDifferentPhrase_ReturnsFalse()
            {
                var other = _word.WithPhrase("other");
                _word.Equals(other).Should().BeFalse();
            }

            [Fact]
            public void GivenDifferentPronunciation_ReturnsFalse()
            {
                var other = _word.WithPronunciation("other");
                _word.Equals(other).Should().BeFalse();
            }
        }

        public class EqualsObject :VocabularyWordTests
        {
            private readonly string _spelling = "bogus";
            private readonly string _phrase = "this is a bogus word.";
            private readonly string _pronunciation = "boogus";

            private readonly VocabularyWord _word;

            public EqualsObject()
            {
                _word = new VocabularyWord(_spelling)
                    .WithPhrase(_phrase)
                    .WithPronunciation(_pronunciation);
            }

            [Fact]
            public void GivenNull_ReturnsFalse()
            {
                object other = null;
                _word.Equals(other).Should().BeFalse();
            }

            [Fact]
            public void GivenSelf_ReturnsTrue()
            {
                object other = _word;
                _word.Equals(other).Should().BeTrue();
            }

            [Fact]
            public void GivenIdentical_ReturnsTrue()
            {
                var other = new VocabularyWord(_spelling)
                    .WithPhrase(_phrase)
                    .WithPronunciation(_pronunciation);
                _word.Equals(other).Should().BeTrue();
            }

            [Fact]
            public void GivenDifferentType_ReturnsTrue()
            {
                _word.Equals(this).Should().BeFalse();
            }
        }

        public class GetHashCodeMethod : VocabularyWordTests
        {
            private readonly string _spelling = "bogus";
            private readonly string _phrase = "this is a bogus word.";
            private readonly string _pronunciation = "boogus";

            private readonly VocabularyWord _word;

            public GetHashCodeMethod()
            {
                _word = new VocabularyWord(_spelling)
                    .WithPhrase(_phrase)
                    .WithPronunciation(_pronunciation);
            }

            [Fact]
            public void GivenSelf_ReturnsConsistentValue()
            {
                _word.GetHashCode().Should().Be(_word.GetHashCode());
            }

            [Fact]
            public void GivenSame_ReturnsConsistentValue()
            {
                var word = new VocabularyWord(_spelling)
                    .WithPhrase(_phrase)
                    .WithPronunciation(_pronunciation);
                word.GetHashCode().Should().Be(_word.GetHashCode());
            }
        }
    }
}
