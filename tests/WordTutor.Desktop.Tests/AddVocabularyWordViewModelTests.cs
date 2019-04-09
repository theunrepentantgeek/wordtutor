using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using WordTutor.Core;
using WordTutor.Desktop.Tests.Fakes;
using Xunit;

namespace WordTutor.Desktop.Tests
{
    public class AddVocabularyWordViewModelTests
    {
        protected readonly FakeApplicationStore _store;
        protected readonly AddVocabularyWordScreen _screen;

        public AddVocabularyWordViewModelTests()
        {
            _screen = new AddVocabularyWordScreen()
                .WithSpelling("spelling")
                .WithPhrase("phrase")
                .WithPronunciation("pronunciation");
            var application = new WordTutorApplication(_screen);
            _store = new FakeApplicationStore(application);
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
            private readonly AddVocabularyWordViewModel _model;

            public AddVocabularyWordViewModelTests()
            {
                _model = new AddVocabularyWordViewModel(_store);
            }

            [Fact]
            public void WhenAssignedCurrentValue_DoesNotFirePropertyChangedEvent()
            {
                var viewModel = new AddVocabularyWordViewModel(_store);
                viewModel.Model.Should().Be(_store.State.CurrentScreen);
            }
        }
    }
}
