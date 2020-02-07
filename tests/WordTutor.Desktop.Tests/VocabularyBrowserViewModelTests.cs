using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using WordTutor.Core;
using WordTutor.Desktop.Tests.Fakes;
using WordTutor.Desktop.Tests.Probes;
using Xunit;

namespace WordTutor.Desktop.Tests
{
    public class VocabularyBrowserViewModelTests
    {
        private readonly FakeApplicationStore _store;
        private readonly FakeLogger _logger = new FakeLogger();
        private readonly WordTutorApplication _application;
        private readonly VocabularyBrowserScreen _screen;
        private readonly VocabularyBrowserViewModel _model;
        private readonly NotifyPropertyChangedProbe _notifyPropertyChanged;

        private readonly VocabularyWord _word = new VocabularyWord("word");
        private readonly VocabularyWord _otherWord = new VocabularyWord("other");

        public VocabularyBrowserViewModelTests()
        {
            _screen = new VocabularyBrowserScreen()
                .WithSelection(_word)
                .MarkAsModified();
            var vocabulary = VocabularySet.Empty
                .Add(_word)
                .Add(_otherWord);

            _application = new WordTutorApplication(_screen)
                .WithVocabularySet(vocabulary);
            _store = new FakeApplicationStore(_application);
            _model = new VocabularyBrowserViewModel(_store, _logger);
            _notifyPropertyChanged = new NotifyPropertyChangedProbe(_model);
            _store.ClearCapturedMessages();
        }

        public class Constructor : VocabularyBrowserViewModelTests
        {
            [Fact]
            public void GivenNullStore_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new VocabularyBrowserViewModel(null!, _logger));
                exception.ParamName.Should().Be("store");
            }

            [Fact]
            public void GivenNullLogger_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new VocabularyBrowserViewModel(_store, null!));
                exception.ParamName.Should().Be("store");
            }

            [Fact]
            public void GivenStore_InitializesPropertiesFromModel()
            {
                var viewModel = new VocabularyBrowserViewModel(_store, _logger);
                viewModel.Selection.Should().Be(_screen.Selection);
                viewModel.Modified.Should().Be(_screen.Modified);
                viewModel.Words.Should().BeEquivalentTo(_application.VocabularySet.Words);
            }
        }

        public class SelectionProperty : VocabularyBrowserViewModelTests
        {
            [Fact]
            public void AssigningValue_ChangesProperty()
            {
                _model.Selection = _otherWord;
                _model.Selection.Should().Be(_otherWord);
            }

            [Fact]
            public void AssigningDifferentValue_SendsExpectedEvent()
            {
                _model.Selection = _otherWord;
                _notifyPropertyChanged.AssertFired(nameof(_model.Selection));
            }

            [Fact]
            public void AssigningExistingValue_DoesNotSendEvent()
            {
                _model.Selection = _word;
                _notifyPropertyChanged.AssertNotFired();
            }
        }

        public class ModifiedProperty: VocabularyBrowserViewModelTests
        {
            [Fact]
            public void AssigningValue_ChangesProperty()
            {
                _model.Modified = false;
                _model.Modified.Should().BeFalse();
            }

            [Fact]
            public void AssigningDifferentValue_SendsExpectedEvent()
            {
                _model.Modified = false;
                _notifyPropertyChanged.AssertFired(nameof(_model.Modified));
            }

            [Fact]
            public void AssigningExistingValue_DoesNotSendEvent()
            {
                _model.Modified = true;
                _notifyPropertyChanged.AssertNotFired();
            }
        }

        public class Words : VocabularyBrowserViewModelTests
        {
        }
    }
}
