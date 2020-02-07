using System;
using System.Collections.ObjectModel;
using System.Linq;
using WordTutor.Core;
using WordTutor.Core.Actions;
using WordTutor.Core.Logging;
using WordTutor.Core.Redux;

namespace WordTutor.Desktop
{
    public sealed class VocabularyBrowserViewModel : ViewModelBase
    {
        private readonly IReduxStore<WordTutorApplication> _store;
        private readonly IDisposable _screenSubscription;
        private readonly IDisposable _vocabularySubscription;
        private readonly IScopedLogger _logger;

        private VocabularyWord? _selection;
        private bool _modified;

        public VocabularyBrowserViewModel(
            IReduxStore<WordTutorApplication> store,
            ILogger logger)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _store = store ?? throw new ArgumentNullException(nameof(store));
            _logger = logger.Action("Browse Vocabulary List");

            var screen = (VocabularyBrowserScreen)_store.State.CurrentScreen;
            var vocab = _store.State.VocabularySet.Words;

            _selection = screen.Selection;
            _modified = screen.Modified;
            Words = new ObservableCollection<VocabularyWord>(
                vocab.OrderBy(w => w.Spelling));

            _screenSubscription = _store.SubscribeToReference(
                app => app.CurrentScreen as VocabularyBrowserScreen,
                RefreshFromScreen);

            _vocabularySubscription = _store.SubscribeToReference(
                app => app.VocabularySet,
                RefreshFromVocabularySet);

            AddWordCommand =
                new RoutedCommandSink(ItemCommands.New, AddWord);

            ModifyWordCommand = new RoutedCommandSink<VocabularyWord>(
                ItemCommands.Open, ModifyWord, CanModifyWord);
        }

        public VocabularyWord? Selection
        {
            get => _selection;
            set => UpdateReferenceProperty(
                ref _selection,
                value,
                sel => _store.Dispatch(CreateSelectionMessage(sel)));
        }

        public bool Modified
        {
            get => _modified;
            set => UpdateValueProperty(ref _modified, value);
        }

        public ObservableCollection<VocabularyWord> Words { get; }

        public RoutedCommandSink AddWordCommand { get; }

        public void AddWord() => _store.Dispatch(new OpenScreenForNewWordMessage());

        public RoutedCommandSink<VocabularyWord> ModifyWordCommand { get; }

        public static bool CanModifyWord(VocabularyWord word)
            => word is VocabularyWord;

        public void ModifyWord(VocabularyWord word)
            => _store.Dispatch(new OpenScreenForModifyingWordMessage(word));

        private void RefreshFromScreen(VocabularyBrowserScreen? screen)
        {
            if (screen == null)
            {
                _screenSubscription.Dispose();
                _vocabularySubscription.Dispose();
                _logger.Dispose();
                return;
            }

            Selection = screen.Selection;
            Modified = screen.Modified;
        }

        private void RefreshFromVocabularySet(VocabularySet? vocabularySet)
        {
            if (vocabularySet is null)
            {
                ClearCollection(Words);
            }
            else
            {
                var words = vocabularySet.Words
                    .OrderBy(w => w.Spelling)
                    .ToList();
                UpdateCollection(Words, words);
            }
        }

        private static IReduxMessage CreateSelectionMessage(VocabularyWord? selection)
        {
            if (selection is null)
            {
                return new ClearSelectedWordMessage();
            }

            return new SelectWordMessage(selection);
        }
    }
}
