using System;
using System.Collections.ObjectModel;
using System.Linq;
using WordTutor.Core;
using WordTutor.Core.Actions;
using WordTutor.Core.Redux;

namespace WordTutor.Desktop
{
    public class VocabularyBrowserViewModel : ViewModelBase
    {
        private readonly IReduxStore<WordTutorApplication> _store;
        private readonly ObservableCollection<VocabularyWord> _words;

        private VocabularyWord _selection;
        private bool _modified;

        public VocabularyBrowserViewModel(IReduxStore<WordTutorApplication> store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _words = new ObservableCollection<VocabularyWord>();
            RefreshFromApplication(_store.State);
        }

        public VocabularyWord Selection
        {
            get => _selection;
            set => UpdateProperty(
                ref _selection,
                value,
                sel => _store.Dispatch(new SelectWordMessage(sel)));
        }

        public bool Modified
        {
            get => _modified;
            set => UpdateProperty(ref _modified, value);
        }

        public ObservableCollection<VocabularyWord> Words
        {
            get => _words;
        }

        private void RefreshFromApplication(WordTutorApplication application)
        {
            var screen = application.CurrentScreen as VocabularyBrowserScreen;
            if (!(screen is null))
            {
                Selection = screen.Selection;
                Modified = screen.Modified;
            }

            if (!(application.VocabularySet is null))
            {
                var words = application.VocabularySet.Words
                    .OrderBy(w => w.Spelling)
                    .ToList();
                UpdateCollection(_words, words);
            }
        }
    }
}
