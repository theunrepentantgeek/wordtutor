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
    public static void Main()
    {
        using var container = CreateContainer();
        var app = new App();

        var factory = container.GetInstance<ViewFactory>();
        var wordTutorModel = container.GetInstance<WordTutorViewModel>();
        var wordTutorWindow = (Window)factory.Create(wordTutorModel);

        app.Run(wordTutorWindow);
    }

    public static Container CreateContainer()
    {
        var container = new Container();
        var coreAssembly = typeof(WordTutorApplication).Assembly;
        var desktopAssembly = typeof(WordTutorWindow).Assembly;

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
