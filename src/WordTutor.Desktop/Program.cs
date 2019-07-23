using SimpleInjector;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using WordTutor.Core;
using WordTutor.Core.Reducers;
using WordTutor.Core.Redux;
using WordTutor.Desktop;

public static class Program
{
    [STAThread]
    static void Main()
    {
        var container = CreateContainer();

        var app = new App();

        var model = container.GetInstance<VocabularyBrowserViewModel>();
        var view = new VocabularyBrowserView
        {
            DataContext = model
        };

        var mainModel = container.GetInstance<WordTutorViewModel>();
        var mainWindow = new MainWindow();
        mainWindow.DataContext = mainModel;

        mainWindow.Shell.Content = view;

        app.Run(mainWindow);
    }

    public static Container CreateContainer()
    {
        var container = new Container();
        var coreAssembly = typeof(WordTutorApplication).Assembly;
        var desktopAssembly = typeof(MainWindow).Assembly;

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

        // Register ViewModels
        container.Collection.Register<ViewModelBase>(desktopAssembly);

        // Register Views
        container.Collection.Register(typeof(UserControl), desktopAssembly);
        container.Collection.Register(typeof(Window), desktopAssembly);

        container.Verify();

        return container;
    }
}
