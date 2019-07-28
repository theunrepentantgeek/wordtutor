using WordTutor.Core;
using WordTutor.Core.Redux;

namespace WordTutor.Desktop
{
    public class WordTutorViewModel : ViewModelBase
    {
        private readonly IReduxStore<WordTutorApplication> _store;
        private readonly ViewModelFactory _factory;

        private ViewModelBase _currentScreen;

        public WordTutorViewModel(
            IReduxStore<WordTutorApplication> store,
            ViewModelFactory factory)
        {
            _store = store;
            _factory = factory;

            UpdateFromStore();
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

        private void UpdateFromStore()
        {
            CurrentScreen = _factory.Create(_store.State.CurrentScreen);
        }
    }
}
