using FluentAssertions;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace WordTutor.Desktop.Tests
{
    public class ViewFactoryTests
    {
        private readonly Container container = Program.CreateContainer();
        private readonly ViewFactory _factory;

        public ViewFactoryTests()
        {
            _factory = container.GetInstance<ViewFactory>();
        }

        [StaFact]
        public void Create_WhenGivenViewModel_ReturnsExpectedView()
        {
            var viewModel = container.GetInstance<VocabularyBrowserViewModel>();
            var view = _factory.Create(viewModel);
            view.Should().NotBeNull();
            view.Should().BeOfType<VocabularyBrowserView>();
        }

        [StaTheory]
        [MemberData(nameof(FindViewModelTypes))]
        public void FindViewType_WhenGivenViewModelType_FindsSuitableViewType(Type viewModelType)
        {
            ViewFactory.FindViewType(viewModelType)
                .Should().NotBeNull($"ViewModelType type {viewModelType.Name} should have a matching View");
        }

        public static IEnumerable<object[]> FindViewModelTypes()
            => from type in typeof(MainWindow).Assembly.GetExportedTypes()
               where type.IsClass && !type.IsAbstract
               where type.Name.EndsWith("ViewModel", StringComparison.Ordinal)
               select new object[] { type };
    }
}
