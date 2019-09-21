using FluentAssertions;
using System;
using WordTutor.Core.Actions;
using WordTutor.Core.Reducers;
using WordTutor.Core.Redux;
using WordTutor.Core.Tests.Fakes;

namespace WordTutor.Core.Tests
{
    public class InteractionTestsBase
    {
        protected VocabularyWord Grave { get; }
            = new VocabularyWord("grave")
                .WithPhrase("Slow and solemn slower than largo");

        protected VocabularyWord Adagio { get; }
            = new VocabularyWord("adagio")
                .WithPhrase("Slow, but not as slow as largo");

        protected VocabularyWord Allegretto { get; }
            = new VocabularyWord("allegretto")
                .WithPhrase("Slightly slower than allegro");

        protected VocabularyWord Moderato { get; }
            = new VocabularyWord("moderato")
                .WithPhrase("At a moderate speed");

        protected VocabularyWord Accelerando { get; }
            = new VocabularyWord("accelerando")
                .WithPhrase("Accelerating");

        protected VocabularyWord Prestissimo { get; }
            = new VocabularyWord("prestissimo")
                .WithPhrase("Very very fast, as fast as possible");

        protected VocabularySet MusicPace { get; }

        protected CompositeReduxReducer<WordTutorApplication> Reducer { get; }
            = new CompositeReduxReducer<WordTutorApplication>(
                new IReduxReducer<WordTutorApplication>[]
                {
                    new ModifyVocabularyWordScreenReducer(),
                    new VocabularyBrowserScreenReducer()
                });

        public InteractionTestsBase()
        {
            MusicPace = VocabularySet.Empty
                .Add(Grave, Adagio, Allegretto, Moderato, Accelerando);
        }

        public static WordTutorApplication ApplicationStateWithVocabulary(VocabularySet words)
            => new WordTutorApplication(new FakeScreen())
                .WithVocabularySet(words);

        public static WordTutorApplication CurrentScreenIs(WordTutorApplication application, Screen screen)
        {
            if (application is null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return application.UpdateScreen<Screen, Screen>(_ => screen);
        }

        public static ModifyVocabularyWordScreen ModifyVocabularyWordScreen { get; }
            = ModifyVocabularyWordScreen.ForNewWord();

        public static VocabularyBrowserScreen VocabularyBrowserScreen { get; } = new VocabularyBrowserScreen();

        public WordTutorApplication TheActionIs(WordTutorApplication application, IReduxMessage message)
            => Reducer.Reduce(message, application);

        public static OpenScreenForNewWordMessage OpenNewWordScreen() => new OpenScreenForNewWordMessage();

        public static SaveNewVocabularyWordMessage SaveNewVocabularyWord(VocabularyWord word)
            => new SaveNewVocabularyWordMessage(word);

        public static void AssertTheCurrentScreenIs<S>(WordTutorApplication application)
            where S : Screen
        {
            if (application is null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            application.CurrentScreen.Should().BeOfType<S>();
        }
    }
}
