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

        var store = container.GetInstance<IReduxStore<WordTutorApplication>>();

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

        // Register Redux Store
        container.RegisterSingleton<
            IReduxStore<WordTutorApplication>,
            ReduxStore<WordTutorApplication>>();
        container.RegisterSingleton<
            IReduxStateFactory<WordTutorApplication>,
            WordTutorApplicationStateFactory>();

        // Register Reducers
        container.RegisterSingleton<
            IReduxReducer<WordTutorApplication>, 
            CompositeReduxReducer<WordTutorApplication>>();
        foreach (var type in container.GetTypesToRegister<IReduxReducer<WordTutorApplication>>(coreAssembly))
        {
            container.Collection.Append(
                typeof(IReduxReducer<WordTutorApplication>),
                type,
                Lifestyle.Singleton);
        }

        container.Verify();

        return container;
    }
}
