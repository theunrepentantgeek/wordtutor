using FluentAssertions;
using WordTutor.Core.Actions;
using WordTutor.Core.Reducers;
using Xunit;

namespace WordTutor.Core.Tests
{
    public class VocabularyBrowserScreenReducerTests
    {
        private readonly VocabularyBrowserScreenReducer _reducer = new VocabularyBrowserScreenReducer();
        private readonly WordTutorApplication _initialState;

        public VocabularyBrowserScreenReducerTests()
        {
            var vocabulary = VocabularySet.Empty
                .Add(Apollo)
                .Add(Ares)
                .Add(Dionysus)
                .Add(Hades);
            var screen =
                new VocabularyBrowserScreen()
                .WithSelection(Ares);
            _initialState = new WordTutorApplication(screen)
                .WithVocabularySet(vocabulary);
        }

        public VocabularyWord Apollo { get; }
            = new VocabularyWord("Apollo")
                .WithPhrase("Apollo was the son of Leto and Zeus");

        public VocabularyWord Ares { get; }
            = new VocabularyWord("Ares")
                .WithPhrase("Ares was the son of Zeus and Hera");

        public VocabularyWord Dionysus { get; }
            = new VocabularyWord("Dionysus")
                .WithPhrase("Dionysus was primarily known as the God of the Vine");

        public VocabularyWord Hades { get; }
            = new VocabularyWord("Hades")
                .WithPhrase("Hades possessed the precious metals of the earth.");

        public VocabularyWord Hephaaestus { get; }
            = new VocabularyWord("Hephaaestus")
                .WithPhrase("Hephaaestus was known as the God of Fire");

        public VocabularyWord Hermes { get; }
            = new VocabularyWord("Hermes")
                .WithPhrase("Hermes was considered a “trickster” due to his cunning and clever personality.");

        public class SelectWordMessageTests : VocabularyBrowserScreenReducerTests
        {
            [Fact]
            public void GivenMessage_ReturnsBrowserScreen()
            {
                var message = new SelectWordMessage(Dionysus);
                var state = _reducer.Reduce(message, _initialState);
                state.CurrentScreen.Should().BeOfType<VocabularyBrowserScreen>();
            }

            [Fact]
            public void GivenMessage_ChangesSelection()
            {
                var message = new SelectWordMessage(Dionysus);
                var state = _reducer.Reduce(message, _initialState);
                var screen = (VocabularyBrowserScreen)state.CurrentScreen;
                screen.Selection.Should().Be(Dionysus);
            }
        }
    }
}
