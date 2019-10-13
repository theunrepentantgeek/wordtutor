using FluentAssertions;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
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
        [SuppressMessage(
            "Design",
            "CA1062:Validate arguments of public methods",
            Justification = "FindViewModelTypes() will never supply a null to this test.")]
        public void FindViewType_WhenGivenViewModelType_FindsSuitableViewType(Type viewModelType)
        {
            if (viewModelType is null)
            {
                throw new ArgumentNullException(nameof(viewModelType));
            }

            ViewFactory.FindViewType(viewModelType)
                .Should().NotBeNull($"ViewModelType type {viewModelType.Name} should have a matching View");
        }

        public static IEnumerable<object[]> FindViewModelTypes()
            => from type in typeof(WordTutorWindow).Assembly.GetExportedTypes()
               where type.IsClass && !type.IsAbstract
               where type.Name.EndsWith("ViewModel", StringComparison.Ordinal)
               select new object[] { type };
    }
}
