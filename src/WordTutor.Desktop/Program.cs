using Microsoft.Extensions.Configuration;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

            var speechMiddleware = container.GetInstance<SpeechMiddleware>();
            store.AddMiddleware(speechMiddleware);

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

            // Register Middleware & Reducers
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

            foreach (var type in container.GetTypesToRegister<IReduxMiddleware>(coreAssembly))
            {
                container.Collection.Append(
                    typeof(IReduxMiddleware),
                    type,
                    Lifestyle.Singleton);
            }

            // Register Services
            container.RegisterSingleton<IRenderSpeechService, AzureSpeechService>();
            container.RegisterDecorator<IRenderSpeechService, CachingRenderSpeechService>(Lifestyle.Singleton);
            container.RegisterSingleton<ISpeechService, SpeechService>();

            // Register ViewModels
            container.RegisterSingleton<ViewModelFactory>();
            container.Collection.Register(typeof(ViewModelBase), desktopAssembly);
            container.RegisterSingleton<WordTutorViewModel>();

            var viewModels =
                from type in desktopAssembly.GetExportedTypes()
                where type.IsAssignableTo(typeof(ViewModelBase))
                where type != typeof(ViewModelBase) && type != typeof(WordTutorViewModel)
                select type;

            foreach (var viewModel in viewModels)
            {
                container.Register(viewModel);
            }

            // Register Views
            container.Register<ViewFactory>();
            container.Collection.Register(typeof(UserControl), desktopAssembly);
            container.Collection.Register(typeof(Window), desktopAssembly);

            var views =
                from type in desktopAssembly.GetExportedTypes()
                where type.IsAssignableTo(typeof(UserControl)) || type.IsAssignableTo(typeof(Window))
                select type;

            foreach (var view in views)
            {
                container.Register(view);
            }

            // Register converters
            container.Register<ViewModelToViewValueConverter>();

            // Register Configuration
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Program>();
            container.RegisterInstance<IConfigurationRoot>(builder.Build());

            return container;
        }
    }
}