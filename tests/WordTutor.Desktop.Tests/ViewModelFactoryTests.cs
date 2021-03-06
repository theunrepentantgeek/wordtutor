﻿using FluentAssertions;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WordTutor.Core;
using Xunit;

namespace WordTutor.Desktop.Tests
{
    public class ViewModelFactoryTests
    {
        private readonly Container _container = Program.CreateContainer();
        private readonly ViewModelFactory _factory;

        public ViewModelFactoryTests()
        {
            _factory = _container.GetInstance<ViewModelFactory>();
        }

        [StaFact]
        public void Create_WhenGivenScreen_ReturnsExpectedViewModel()
        {
            var screen = ModifyVocabularyWordScreen.ForNewWord();
            var viewModel = _factory.Create(screen);
            viewModel.Should().NotBeNull();
            viewModel.Should().BeOfType<ModifyVocabularyWordViewModel>();
        }

        [StaTheory]
        [MemberData(nameof(FindScreenTypes))]
        [SuppressMessage(
            "Design",
            "CA1062:Validate arguments of public methods",
            Justification = "FindScreenTypes() will never supply a null to this test method.")]
        public void FindViewModelType_WhenGivenScreenType_FindsSuitableViewModelType(Type screenType)
        {
            ViewModelFactory.FindViewModelType(screenType)
                .Should().NotBeNull($"Screen type {screenType.Name} should have a matching ViewModel");
        }

        public static IEnumerable<object[]> FindScreenTypes()
            => from type in typeof(WordTutorApplication).Assembly.GetExportedTypes()
               where type.IsClass && !type.IsAbstract
               where type.Name.EndsWith("Screen", StringComparison.Ordinal)
               select new object[] { type };
    }
}
