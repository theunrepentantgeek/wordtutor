using SimpleInjector;
using SimpleInjector.Diagnostics;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using WordTutor.Core;
using WordTutor.Core.Logging;
using WordTutor.Core.Reducers;
using WordTutor.Core.Redux;
using WordTutor.Desktop;

namespace WordTutor
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using var container = CreateContainer();
            var app = new App();

            var factory = container.GetInstance<ViewFactory>();
            using var wordTutorModel = container.GetInstance<WordTutorViewModel>();
            var wordTutorWindow = (Window)factory.Create(wordTutorModel);

            app.Run(wordTutorWindow);
        }

        public static Container CreateContainer()
        {
            var container = new Container();
            var coreAssembly = typeof(WordTutorApplication).Assembly;
            var desktopAssembly = typeof(WordTutorWindow).Assembly;

            // General Infrastructure
            container.RegisterSingleton<ILogger, Logger>();

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

            // Suppress Warnings
            var registration = container.GetRegistration(typeof(WordTutorViewModel))!.Registration;
            registration.SuppressDiagnosticWarning(
                DiagnosticType.DisposableTransientComponent,
                "WordTutorViewModel is disposed in Main()");

            container.Verify();

            return container;
        }
    }
}