using System;
using WordTutor.Core;
using WordTutor.Core.Actions;
using WordTutor.Core.Redux;

namespace WordTutor.Desktop
{
    public class AddVocabularyWordViewModel : ViewModelBase
    {
        private readonly IReduxStore<WordTutorApplication> _store;

        private string _spelling;
        private string _phrase;
        private string _pronunciation;

        public AddVocabularyWordViewModel(IReduxStore<WordTutorApplication> store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            UpdateFromStore();
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

        private void UpdateFromStore()
        {
            var model = _store.State.CurrentScreen as AddVocabularyWordScreen;

            if (!(model is null))
            {
                Spelling = model?.Spelling ?? string.Empty;
                Phrase = model?.Phrase ?? string.Empty;
                Pronunciation = model?.Pronunciation ?? string.Empty;
            }
        }
    }
}
