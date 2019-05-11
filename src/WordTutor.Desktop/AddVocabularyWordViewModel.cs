using System;
using WordTutor.Core;
using WordTutor.Core.Actions;
using WordTutor.Core.Redux;

namespace WordTutor.Desktop
{
    public class AddVocabularyWordViewModel : ViewModelBase<AddVocabularyWordScreen>
    {
        private readonly IReduxStore<WordTutorApplication> _store;

        private string _spelling;
        private string _phrase;
        private string _pronunciation;

        public AddVocabularyWordViewModel(IReduxStore<WordTutorApplication> store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            Model = store.State.CurrentScreen as AddVocabularyWordScreen;
        }

        public string Spelling
        {
            get => _spelling;
            set
            {
                if (UpdateProperty(ref _spelling, value))
                {
                    _store.Dispatch(new ModifySpellingMessage(_spelling));
                }
            }
        }

        public string Phrase
        {
            get => _phrase;
            set
            {
                if (UpdateProperty(ref _phrase, value))
                {
                    _store.Dispatch(new ModifyPhraseMessage(_phrase));
                }
            }
        }

        public string Pronunciation
        {
            get => _pronunciation;
            set
            {
                if (UpdateProperty(ref _pronunciation, value))
                {
                    _store.Dispatch(new ModifyPronunciationMessage(_pronunciation));
                }
            }
        }

        protected override void ModelUpdated(AddVocabularyWordScreen model)
        {
            Spelling = model?.Spelling ?? string.Empty;
            Phrase = model?.Phrase ?? string.Empty;
            Pronunciation = model?.Pronunciation ?? string.Empty;
        }
    }
}
