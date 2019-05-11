using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using WordTutor.Core.Actions;
using Xunit;

namespace WordTutor.Core.Tests
{
    public class ScreenReducerTests
    {
        private readonly ScreenReducer _screenReducer = new ScreenReducer();

        public class ScreenIsAddVocabularyWord : ScreenReducerTests
        {
            private readonly AddVocabularyWordScreen _screen =
                new AddVocabularyWordScreen()
                    .WithSpelling("Phoo")
                    .WithPronunciation("Foo")
                    .WithPhrase("Phrase");

            [Fact]
            public void GivenModifySpellingMessage_ReturnsAddVocabularyWordScreen()
            {
                var message = new ModifySpellingMessage("Bar");
                var screen = _screenReducer.Reduce(message, _screen);
                screen.Should().BeOfType<AddVocabularyWordScreen>();
            }

            [Fact]
            public void GivenModifySpellingMessage_ModifiesExpectedProperty()
            {
                var message = new ModifySpellingMessage("Bar");
                var screen = (AddVocabularyWordScreen)_screenReducer.Reduce(message, _screen);
                screen.Spelling.Should().Be(message.Spelling);
            }

            [Fact]
            public void GivenModifyPhraseMessage_ReturnsAddVocabularyWordScreen()
            {
                var message = new ModifyPhraseMessage("Bar");
                var screen = _screenReducer.Reduce(message, _screen);
                screen.Should().BeOfType<AddVocabularyWordScreen>();
            }

            [Fact]
            public void GivenModifyPhraseMessage_ModifiesExpectedProperty()
            {
                var message = new ModifyPhraseMessage("Bar");
                var screen = (AddVocabularyWordScreen)_screenReducer.Reduce(message, _screen);
                screen.Phrase.Should().Be(message.Phrase);
            }

            [Fact]
            public void GivenModifyPronunciationMessage_ReturnsAddVocabularyWordScreen()
            {
                var message = new ModifyPronunciationMessage("Bar");
                var screen = _screenReducer.Reduce(message, _screen);
                screen.Should().BeOfType<AddVocabularyWordScreen>();
            }

            [Fact]
            public void GivenModifyPronunciationMessage_ModifiesExpectedProperty()
            {
                var message = new ModifyPronunciationMessage("Bar");
                var screen = (AddVocabularyWordScreen)_screenReducer.Reduce(message, _screen);
                screen.Pronunciation.Should().Be(message.Pronunciation);
            }
        }
    }
}
