using System;
using WordTutor.Core;
using WordTutor.Core.Redux;

namespace WordTutor.Desktop
{
    public sealed class WordTutorViewModel : ViewModelBase, IDisposable
    {
        private readonly IReduxStore<WordTutorApplication> _store;
        private readonly ViewModelFactory _factory;
        private readonly IDisposable _screenSubscription;

        private ViewModelBase _currentScreen;

        public WordTutorViewModel(
            IReduxStore<WordTutorApplication> store,
            ViewModelFactory factory)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));

            _currentScreen = _factory.Create(_store.State.CurrentScreen);

            _screenSubscription = _store.SubscribeToReference(
                app => app.CurrentScreen,
                RefreshFromScreen);
        }

        public ViewModelBase CurrentScreen
        {
            get => _currentScreen;
            set
            {
                if (!ReferenceEquals(value, _currentScreen))
                {
                    _currentScreen = value;
                    OnPropertyChanged(nameof(CurrentScreen));
                }
            }
        }

        public void Dispose()
        {
            _screenSubscription?.Dispose();
        }

        private void RefreshFromScreen(Screen? screen)
        {
            if (screen is null)
            {
                return;
            }

            // Only need a new instance if the screen type changes
            // As long as the type of screen is unchanged, 
            // the exiting ViewModel will update

            var neededType = ViewModelFactory.FindViewModelType(screen.GetType());
            if (CurrentScreen?.GetType() != neededType)
            {
                CurrentScreen = _factory.Create(screen);
            }
        }
    }
}
