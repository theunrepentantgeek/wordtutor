using Microsoft.Extensions.Configuration;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using System;
using System.Windows;
using System.Windows.Controls;
using WordTutor.Azure;
using WordTutor.Core;
using WordTutor.Core.Logging;
using WordTutor.Core.Redux;
using WordTutor.Core.Services;
using WordTutor.Desktop;

namespace WordTutor
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            using var container = CreateContainer();
            var app = new App();

            var store = (ReduxStore<WordTutorApplication>)container.GetInstance<IReduxStore<WordTutorApplication>>();
            store.ClearSubscriptions();

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
            container.RegisterSingleton<ILogger, DelegatingLogger>();

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

            // Register Services
            container.RegisterSingleton<ISpeechService, AzureSpeechService>();

            // Register ViewModels
            container.Collection.Register<ViewModelBase>(desktopAssembly);

            // Register Views
            container.Collection.Register(typeof(UserControl), desktopAssembly);
            container.Collection.Register(typeof(Window), desktopAssembly);

            // Register Configuration
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Program>();
            container.RegisterInstance<IConfigurationRoot>(builder.Build());

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