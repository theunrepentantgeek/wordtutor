using WordTutor.Core;
using WordTutor.Core.Redux;

namespace WordTutor.Desktop
{
    public class WordTutorApplicationStateFactory : IReduxStateFactory<WordTutorApplication>
    {
        public WordTutorApplication Create()
        {
            var alpha = new VocabularyWord("alpha")
                .WithPhrase("The alpha dog")
                .WithPronunciation("alfa");

            var beta = new VocabularyWord("beta")
                .WithPhrase("A beta release")
                .WithPronunciation("beta");

            var gamma = new VocabularyWord("gamma")
                .WithPhrase("Gamma radiation")
                .WithPronunciation("gamma");

            var delta = new VocabularyWord("delta")
                .WithPhrase("Change is often called delta.")
                .WithPronunciation("delta");

            var vocabulary = VocabularySet.Empty
                .Add(alpha)
                .Add(beta)
                .Add(gamma)
                .Add(delta);

            var screen = new VocabularyBrowserScreen()
                .WithSelection(gamma);

            var application = new WordTutorApplication(screen)
                .WithVocabularySet(vocabulary);

            return application;
        }
    }
}
