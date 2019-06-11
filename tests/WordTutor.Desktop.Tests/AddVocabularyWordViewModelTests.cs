using System;

using FluentAssertions;
using WordTutor.Core;
using WordTutor.Core.Actions;
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
            _store.ClearCapturedMessages();
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
            public void GivenStore_InitializesPropertiesFromModel()
            {
                var viewModel = new AddVocabularyWordViewModel(_store);
                viewModel.Spelling.Should().Be(_screen.Spelling);
                viewModel.Phrase.Should().Be(_screen.Phrase);
                viewModel.Pronunciation.Should().Be(_screen.Pronunciation);
            }
        }

        public class SpellingProperty : AddVocabularyWordViewModelTests
        {
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

            [Fact]
            public void AssigningDifferentValue_SendExpectedMessage()
            {
                _model.Spelling = "word";
                var message = _store.AssertReceived<ModifySpellingMessage>();
                message.Spelling.Should().Be(_model.Spelling);
            }
        }

        public class PhraseProperty : AddVocabularyWordViewModelTests
        {
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

            [Fact]
            public void AssigningDifferentValue_SendExpectedMessage()
            {
                _model.Phrase = "this is a word";
                var message = _store.AssertReceived<ModifyPhraseMessage>();
                message.Phrase.Should().Be(_model.Phrase);
            }
        }

        public class PronunciationProperty : AddVocabularyWordViewModelTests
        {
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

            [Fact]
            public void AssigningDifferentValue_SendExpectedMessage()
            {
                _model.Pronunciation = "this is a word";
                var message = _store.AssertReceived<ModifyPronunciationMessage>();
                message.Pronunciation.Should().Be(_model.Pronunciation);
            }
        }
    }
}
