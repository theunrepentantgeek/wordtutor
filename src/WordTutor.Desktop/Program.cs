using SimpleInjector;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using WordTutor.Core;
using WordTutor.Core.Reducers;
using WordTutor.Core.Redux;
using WordTutor.Desktop;

static partial class Program
{
    [STAThread]
    static void Main()
    {
        var container = CreateContainer();

        var app = new App();

        var store = container.GetInstance<ReduxStore<WordTutorApplication>>();

        var model = new VocabularyBrowserViewModel(store);
        var view = new VocabularyBrowserView
        {
            DataContext = model
        };

        var mainWindow = new MainWindow();
        mainWindow.Shell.Content = view;

        app.Run(mainWindow);
    }

    private static Container CreateContainer()
    {
        var container = new Container();
        var coreAssembly = typeof(WordTutorApplication).Assembly;
        var desktopAssembly = Assembly.GetExecutingAssembly();

        // Register Redux 
        container.RegisterSingleton<
            IReduxStore<WordTutorApplication>,
            ReduxStore<WordTutorApplication>>();
        container.RegisterSingleton<
            IReduxStateFactory<WordTutorApplication>,
            WordTutorApplicationStateFactory>();

        container.Verify();

        return container;
    }
}
