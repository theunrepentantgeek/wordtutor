using FluentAssertions;
using System;
using System.ComponentModel;
using WordTutor.Core;
using WordTutor.Desktop.Tests.Fakes;
using WordTutor.Desktop.Tests.Probes;
using Xunit;

namespace WordTutor.Desktop.Tests
{
    public class AddVocabularyWordViewModelTests
    {
        private readonly FakeApplicationStore _store;
        private readonly AddVocabularyWordScreen _screen;
        private readonly AddVocabularyWordViewModel _model;
        private readonly NotifyPropertyChangedProbe _notifyPropertyChanged;

        public AddVocabularyWordViewModelTests()
        {
            _screen = new AddVocabularyWordScreen()
                .WithSpelling("spelling")
                .WithPhrase("phrase")
                .WithPronunciation("pronunciation");
            var application = new WordTutorApplication(_screen);
            _store = new FakeApplicationStore(application);
            _model = new AddVocabularyWordViewModel(_store);
            _notifyPropertyChanged = new NotifyPropertyChangedProbe(_model);
        }

        public class Constructor : AddVocabularyWordViewModelTests
        {
            [Fact]
            public void GivenNullStore_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new AddVocabularyWordViewModel(null));
                exception.ParamName.Should().Be("store");
            }

            [Fact]
            public void GivenStore_InitializesModelProperty()
            {
                var viewModel = new AddVocabularyWordViewModel(_store);
                viewModel.Model.Should().Be(_store.State.CurrentScreen);
            }

            [Fact]
            public void GivenStore_InitializesSpellingProperty()
            {
                var viewModel = new AddVocabularyWordViewModel(_store);
                viewModel.Spelling.Should().Be(_screen.Spelling);
            }

            [Fact]
            public void GivenStore_InitializesPhraseProperty()
            {
                var viewModel = new AddVocabularyWordViewModel(_store);
                viewModel.Phrase.Should().Be(_screen.Phrase);
            }

            [Fact]
            public void GivenStore_InitializesPronunciationProperty()
            {
                var viewModel = new AddVocabularyWordViewModel(_store);
                viewModel.Pronunciation.Should().Be(_screen.Pronunciation);
            }
        }

        public class SpellingProperty : AddVocabularyWordViewModelTests
        {
            private readonly string _spellingProperty =
                nameof(AddVocabularyWordViewModel.Spelling);

            [Fact]
            public void AssigningValue_ChangesProperty()
            {
                const string value = "new";
                _model.Spelling = value;
                _model.Spelling.Should().Be(value);
            }

            [Fact]
            public void AssigningExistingValue_DoesNotSendEvent()
            {
                _model.Spelling = _model.Spelling;
                _notifyPropertyChanged.AssertNotFired();
            }

            [Fact]
            public void AssigningDifferentValue_SendsExpectedEvent()
            {
                _model.Spelling = "word";
                _notifyPropertyChanged.AssertFired(nameof(_model.Spelling));
            }
        }

        public class PhraseProperty : AddVocabularyWordViewModelTests
        {
            private readonly string _phraseProperty =
                nameof(AddVocabularyWordViewModel.Phrase);

            [Fact]
            public void AssigningValue_ChangesProperty()
            {
                const string value = "new";
                _model.Phrase = value;
                _model.Phrase.Should().Be(value);
            }

            [Fact]
            public void AssigningExistingValue_DoesNotSendEvent()
            {
                _model.Phrase = _model.Phrase;
                _notifyPropertyChanged.AssertNotFired();
            }

            [Fact]
            public void AssigningDifferentValue_SendsExpectedEvent()
            {
                _model.Phrase = "new";
                _notifyPropertyChanged.AssertFired(nameof(_model.Phrase));
            }
        }

        public class PronunciationProperty : AddVocabularyWordViewModelTests
        {
            private readonly string _pronunciationProperty =
                nameof(AddVocabularyWordViewModel.Pronunciation);

            [Fact]
            public void AssigningValue_ChangesProperty()
            {
                const string value = "new";
                _model.Pronunciation = value;
                _model.Pronunciation.Should().Be(value);
            }

            [Fact]
            public void AssigningExistingValue_DoesNotSendEvent()
            {
                _model.Pronunciation = _model.Pronunciation;
                _notifyPropertyChanged.AssertNotFired();
            }

            [Fact]
            public void AssigningDifferentValue_SendsExpectedEvent()
            {
                _model.Pronunciation = "new";
                _notifyPropertyChanged.AssertFired(nameof(_model.Pronunciation));
            }
        }
    }
}
