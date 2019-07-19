using FluentAssertions;
using WordTutor.Core.Actions;
using WordTutor.Core.Reducers;
using Xunit;

namespace WordTutor.Core.Tests
{
    public class AddVocabularyWordScreenReducerTests
    {
        private readonly AddVocabularyWordScreenReducer _reducer = new AddVocabularyWordScreenReducer();
        private readonly WordTutorApplication _initialState;

        public AddVocabularyWordScreenReducerTests()
        {
            var screen =
                new AddVocabularyWordScreen()
                    .WithSpelling("Phoo")
                    .WithPronunciation("Foo")
                    .WithPhrase("Phrase");
            _initialState = new WordTutorApplication(screen);
        }

        public class ModifySpellingMessageTests : AddVocabularyWordScreenReducerTests
        {
            [Fact]
            public void GivenMessage_ReturnsAddVocabularyWordScreen()
            {
                var message = new ModifySpellingMessage("Bar");
                var state = _reducer.Reduce(message, _initialState);
                state.CurrentScreen.Should().BeOfType<AddVocabularyWordScreen>();
            }

            [Fact]
            public void GivenMessage_ModifiesExpectedProperty()
            {
                var message = new ModifySpellingMessage("Bar");
                var state = _reducer.Reduce(message, _initialState);
                var screen = (AddVocabularyWordScreen)state.CurrentScreen;
                screen.Spelling.Should().Be(message.Spelling);
            }
        }

        public class ModifyPhraseMessageTests : AddVocabularyWordScreenReducerTests
        {
            [Fact]
            public void GivenMessage_ReturnsAddVocabularyWordScreen()
            {
                var message = new ModifyPhraseMessage("Bar");
                var state = _reducer.Reduce(message, _initialState);
                state.CurrentScreen.Should().BeOfType<AddVocabularyWordScreen>();
            }

            [Fact]
            public void GivenMessage_ModifiesExpectedProperty()
            {
                var message = new ModifyPhraseMessage("Bar");
                var state = _reducer.Reduce(message, _initialState);
                var screen = (AddVocabularyWordScreen)state.CurrentScreen;
                screen.Phrase.Should().Be(message.Phrase);
            }
        }

        public class ModifyPronunciationMessageTests : AddVocabularyWordScreenReducerTests
        {
            [Fact]
            public void GivenMessage_ReturnsAddVocabularyWordScreen()
            {
                var message = new ModifyPronunciationMessage("Bar");
                var state = _reducer.Reduce(message, _initialState);
                var screen = state.CurrentScreen;
                screen.Should().BeOfType<AddVocabularyWordScreen>();
            }

            [Fact]
            public void GivenMessage_ModifiesExpectedProperty()
            {
                var message = new ModifyPronunciationMessage("Bar");
                var state = _reducer.Reduce(message, _initialState);
                var screen = (AddVocabularyWordScreen)state.CurrentScreen;
                screen.Pronunciation.Should().Be(message.Pronunciation);
            }
        }
    }
}
