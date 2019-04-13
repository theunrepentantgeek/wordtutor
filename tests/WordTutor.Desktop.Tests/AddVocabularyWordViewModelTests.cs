using FluentAssertions;
using System;
using System.ComponentModel;
using WordTutor.Core;
using WordTutor.Desktop.Tests.Fakes;
using Xunit;

namespace WordTutor.Desktop.Tests
{
    public class AddVocabularyWordViewModelTests
    {
        private readonly FakeApplicationStore _store;
        private readonly AddVocabularyWordScreen _screen;
        private readonly AddVocabularyWordViewModel _model;

        public AddVocabularyWordViewModelTests()
        {
            _screen = new AddVocabularyWordScreen()
                .WithSpelling("spelling")
                .WithPhrase("phrase")
                .WithPronunciation("pronunciation");
            var application = new WordTutorApplication(_screen);
            _store = new FakeApplicationStore(application);
            _model = new AddVocabularyWordViewModel(_store);
        }

        private void Assert_AssigningValue_ChangesProperty<T>(
            T model, string propertyName, string value)
            where T : INotifyPropertyChanged
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            propertyInfo.Should().NotBeNull("can only test known public properties.");

            propertyInfo.SetValue(model, value);
            var propertyValue = propertyInfo.GetValue(model);

            propertyValue.Should().Be(value);
        }

        private void Assert_AssigningExistingValue_DoesNotSendEvent<T>(
            T model, string propertyName)
            where T : INotifyPropertyChanged
        {
            PropertyChangedEventArgs args = null;
            var propertyInfo = typeof(T).GetProperty(propertyName);
            propertyInfo.Should().NotBeNull("can only test known public properties.");
            var existingValue = propertyInfo.GetValue(model);
            model.PropertyChanged += (s, a) => args = a;

            propertyInfo.SetValue(model, existingValue);

            args.Should().BeNull();
        }

        private void Assert_AssigningDifferentValue_SendsExpectedEvent<T>(
            T model, string propertyName, string newValue)
            where T : INotifyPropertyChanged
        {
            PropertyChangedEventArgs args = null;
            var propertyInfo = typeof(T).GetProperty(propertyName);
            propertyInfo.Should().NotBeNull("can only test known public properties.");
            var existingValue = propertyInfo.GetValue(model);
            model.PropertyChanged += (s, a) => args = a;

            propertyInfo.SetValue(model, newValue);

            args.Should().NotBeNull();
            args.PropertyName.Should().Be(propertyName);
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
                => Assert_AssigningValue_ChangesProperty(
                    _model, _spellingProperty, "new");

            [Fact]
            public void AssigningExistingValue_DoesNotSendEvent()
                => Assert_AssigningExistingValue_DoesNotSendEvent(
                    _model, _spellingProperty);

            [Fact]
            public void AssigningDifferentValue_SendsExpectedEvent()
                => Assert_AssigningDifferentValue_SendsExpectedEvent(
                    _model, _spellingProperty, "word");
        }

        public class PhraseProperty : AddVocabularyWordViewModelTests
        {
            private readonly string _phraseProperty =
                nameof(AddVocabularyWordViewModel.Phrase);

            [Fact]
            public void AssigningValue_ChangesProperty()
                => Assert_AssigningValue_ChangesProperty(
                    _model, _phraseProperty, "new");

            [Fact]
            public void AssigningExistingValue_DoesNotSendEvent()
                => Assert_AssigningExistingValue_DoesNotSendEvent(
                    _model, _phraseProperty);

            [Fact]
            public void AssigningDifferentValue_SendsExpectedEvent()
                => Assert_AssigningDifferentValue_SendsExpectedEvent(
                    _model, _phraseProperty, "word");
        }

        public class PronunciationProperty : AddVocabularyWordViewModelTests
        {
            private readonly string _pronunciationProperty =
                nameof(AddVocabularyWordViewModel.Pronunciation);

            [Fact]
            public void AssigningValue_ChangesProperty()
                => Assert_AssigningValue_ChangesProperty(
                    _model, _pronunciationProperty, "new");

            [Fact]
            public void AssigningExistingValue_DoesNotSendEvent()
                => Assert_AssigningExistingValue_DoesNotSendEvent(
                    _model, _pronunciationProperty);

            [Fact]
            public void AssigningDifferentValue_SendsExpectedEvent()
                => Assert_AssigningDifferentValue_SendsExpectedEvent(
                    _model, _pronunciationProperty, "word");
        }
    }
}
