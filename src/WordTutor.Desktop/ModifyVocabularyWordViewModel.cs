using System;
using WordTutor.Core;
using WordTutor.Core.Actions;
using WordTutor.Core.Redux;

namespace WordTutor.Desktop
{
    public class ModifyVocabularyWordViewModel : ViewModelBase
    {
        private readonly IReduxStore<WordTutorApplication> _store;
        private readonly IDisposable _screenSubscription;

        private string _spelling;
        private string _phrase;
        private string _pronunciation;
        private string _caption;

        public ModifyVocabularyWordViewModel(IReduxStore<WordTutorApplication> store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));

            var screen = _store.State.CurrentScreen as ModifyVocabularyWordScreen;
            _spelling = screen?.Spelling ?? string.Empty;
            _phrase = screen?.Phrase ?? string.Empty;
            _pronunciation = screen?.Pronunciation ?? string.Empty;
            _caption = screen?.ExistingWord is null ? "Add Word" : "Modify Word";

            _screenSubscription = _store.SubscribeToReference(
                app => app.CurrentScreen as ModifyVocabularyWordScreen,
                RefreshFromScreen);
        }

        public string Spelling
        {
            get => _spelling;
            set => UpdateReferenceProperty(
                ref _spelling,
                value,
                sp => _store.Dispatch(new ModifySpellingMessage(sp)));
        }

        public string Phrase
        {
            get => _phrase;
            set => UpdateReferenceProperty(
                ref _phrase,
                value,
                ph => _store.Dispatch(new ModifyPhraseMessage(ph)));
        }

        public string Pronunciation
        {
            get => _pronunciation;
            set => UpdateReferenceProperty(
                ref _pronunciation,
                value,
                pr => _store.Dispatch(new ModifyPronunciationMessage(pr)));
        }

        public string Caption
        {
            get => _caption;
            set => UpdateReferenceProperty(
                ref _caption,
                value);
        }

        public ModifyVocabularyWordScreen? Screen
            => _store.State.CurrentScreen as ModifyVocabularyWordScreen;

        public RoutedCommandSink SaveCommand { get; }

        public bool CanSave() => Screen?.Modified ?? false;

        public void Save()
        {
            var word = Screen!.AsWord();
            var existingWord = Screen!.ExistingWord;

            IReduxMessage message;
            if (existingWord is null)
            {
                message = new SaveNewVocabularyWordMessage(word);
            }
            else
            {
                message = new SaveModifiedVocabularyWordMessage(existingWord, word);
            }

            _store.Dispatch(message);
        }

        public RoutedCommandSink CloseCommand { get; }

        public void Close()
        {
            _store.Dispatch(new CloseScreenMessage());
        }

        private void RefreshFromScreen(ModifyVocabularyWordScreen? screen)
        {
            if (screen is null)
            {
                _screenSubscription.Dispose();
                return;
            }

            Spelling = screen.Spelling ?? string.Empty;
            Phrase = screen.Phrase ?? string.Empty;
            Pronunciation = screen.Pronunciation ?? string.Empty;
            Caption = screen.ExistingWord is null ? "Add Word" : "Modify Word";
        }
    }
}
