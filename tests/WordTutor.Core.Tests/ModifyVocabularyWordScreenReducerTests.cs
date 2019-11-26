using FluentAssertions;
using WordTutor.Core.Actions;
using WordTutor.Core.Reducers;
using Xunit;

namespace WordTutor.Core.Tests
{
    public class ModifyVocabularyWordScreenReducerTests
    {
        private readonly ModifyVocabularyWordScreenReducer _reducer = new ModifyVocabularyWordScreenReducer();
        private readonly WordTutorApplication _initialState;

        public ModifyVocabularyWordScreenReducerTests()
        {
            var screen =
                ModifyVocabularyWordScreen.ForNewWord()
                    .WithSpelling("Phoo")
                    .WithPronunciation("Foo")
                    .WithPhrase("Phrase");
            _initialState = new WordTutorApplication(screen);
        }

        public class ModifySpellingMessageTests : ModifyVocabularyWordScreenReducerTests
        {
            [Fact]
            public void GivenMessage_ReturnsMaintainVocabularyWordScreen()
            {
                var message = new ModifySpellingMessage("Bar");
                var state = _reducer.Reduce(message, _initialState);
                state.CurrentScreen.Should().BeOfType<ModifyVocabularyWordScreen>();
            }

            [Fact]
            public void GivenMessage_ModifiesExpectedProperty()
            {
                var message = new ModifySpellingMessage("Bar");
                var state = _reducer.Reduce(message, _initialState);
                var screen = (ModifyVocabularyWordScreen)state.CurrentScreen;
                screen.Spelling.Should().Be(message.Spelling);
            }
        }

        public class ModifyPhraseMessageTests : ModifyVocabularyWordScreenReducerTests
        {
            [Fact]
            public void GivenMessage_ReturnsMaintainVocabularyWordScreen()
            {
                var message = new ModifyPhraseMessage("Bar");
                var state = _reducer.Reduce(message, _initialState);
                state.CurrentScreen.Should().BeOfType<ModifyVocabularyWordScreen>();
            }

            [Fact]
            public void GivenMessage_ModifiesExpectedProperty()
            {
                var message = new ModifyPhraseMessage("Bar");
                var state = _reducer.Reduce(message, _initialState);
                var screen = (ModifyVocabularyWordScreen)state.CurrentScreen;
                screen.Phrase.Should().Be(message.Phrase);
            }
        }

        public class ModifyPronunciationMessageTests : ModifyVocabularyWordScreenReducerTests
        {
            [Fact]
            public void GivenMessage_ReturnsMaintainVocabularyWordScreen()
            {
                var message = new ModifyPronunciationMessage("Bar");
                var state = _reducer.Reduce(message, _initialState);
                var screen = state.CurrentScreen;
                screen.Should().BeOfType<ModifyVocabularyWordScreen>();
            }

            [Fact]
            public void GivenMessage_ModifiesExpectedProperty()
            {
                var message = new ModifyPronunciationMessage("Bar");
                var state = _reducer.Reduce(message, _initialState);
                var screen = (ModifyVocabularyWordScreen)state.CurrentScreen;
                screen.Pronunciation.Should().Be(message.Pronunciation);
            }
        }
    }
}
