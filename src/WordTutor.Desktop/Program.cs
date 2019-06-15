using System;
using System.Windows;
using WordTutor.Core;
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
        var model = new AddVocabularyWordViewModel(store);
        var view = new AddVocabularyWordView
        {
            DataContext = model
        };

        var mainWindow = new MainWindow();
        mainWindow.Shell.Content = view;

        app.Run(mainWindow);
    }

    private static IReduxStore<WordTutorApplication> CreateStore(WordTutorApplication application)
    {
        var screenReducer = new ScreenReducer();
        var appReducer = new WordTutorAppicationReducer(screenReducer);
        var store = new ReduxStore<WordTutorApplication>(appReducer, application);
        return new LoggingReduxStore<WordTutorApplication>(store);
    }

    private static WordTutorApplication CreateApplicationModel()
    {
        var screen = new AddVocabularyWordScreen()
            .WithSpelling("Demo")
            .WithPhrase("This is a demo phrase")
            .WithPronunciation("Deemoe");

        var application = new WordTutorApplication(screen);
        return application;
    }
}
