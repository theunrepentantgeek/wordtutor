using System;
using System.Windows;
using WordTutor.Core;
using WordTutor.Core.Reducers;
using WordTutor.Core.Redux;
using WordTutor.Desktop;

static class Program
{
    [STAThread]
    static void Main()
    {
        var app = new App();

        var application = CreateApplicationModel();
        var store = CreateStore(application);
        var model = new VocabularyBrowserViewModel(store);
        var view = new VocabularyBrowserView
        {
            DataContext = model
        };

        var mainWindow = new MainWindow();
        mainWindow.Shell.Content = view;

        app.Run(mainWindow);
    }

    private static IReduxStore<WordTutorApplication> CreateStore(WordTutorApplication application)
    {
        var reducer = new CompositeReduxReducer<WordTutorApplication>(
            new IReduxReducer<WordTutorApplication>[]
            {
                new AddVocabularyWordScreenReducer(),
                new VocabularyBrowserScreenReducer()
            });

        var store = new ReduxStore<WordTutorApplication>(reducer, application);
        return new LoggingReduxStore<WordTutorApplication>(store);
    }

    private static WordTutorApplication CreateApplicationModel()
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
