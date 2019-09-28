using System;
using WordTutor.Core;
using WordTutor.Core.Actions;
using WordTutor.Core.Redux;

namespace WordTutor.Desktop
{
    public class AddVocabularyWordViewModel : ViewModelBase
    {
        private readonly IReduxStore<WordTutorApplication> _store;
        private readonly IDisposable _screenSubscription;

        private string _spelling;
        private string _phrase;
        private string _pronunciation;

        public AddVocabularyWordViewModel(IReduxStore<WordTutorApplication> store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));

            var screen = _store.State.CurrentScreen as AddVocabularyWordScreen;
            _spelling = screen?.Spelling ?? string.Empty;
            _phrase = screen?.Phrase ?? string.Empty;
            _pronunciation = screen?.Pronunciation ?? string.Empty;

            _screenSubscription = _store.Subscribe(
                app => app.CurrentScreen as AddVocabularyWordScreen,
                RefreshFromScreen);
        }

        public string Spelling
        {
            get => _spelling;
            set => UpdateProperty(
                ref _spelling,
                value,
                sp => _store.Dispatch(new ModifySpellingMessage(sp)));
        }

        public string Phrase
        {
            get => _phrase;
            set => UpdateProperty(
                ref _phrase,
                value,
                ph => _store.Dispatch(new ModifyPhraseMessage(ph)));
        }

        public string Pronunciation
        {
            get => _pronunciation;
            set => UpdateProperty(
                ref _pronunciation,
                value,
                pr => _store.Dispatch(new ModifyPronunciationMessage(pr)));
        }

        private void RefreshFromScreen(AddVocabularyWordScreen? screen)
        {
            if (screen is null)
            {
                _screenSubscription.Dispose();
                return;
            }

            Spelling = screen.Spelling ?? string.Empty;
            Phrase = screen.Phrase ?? string.Empty;
            Pronunciation = screen.Pronunciation ?? string.Empty;
        }
    }
}
