﻿using Xunit;

using static Niche.GherkinSyntax.GherkinFactory;

namespace WordTutor.Core.Tests
{
    public class VocabularyMaintenanceInteractionTests : InteractionTestsBase
    {
        [Fact]
        public void WhenStartingNewVocabularyWord_MaintainVocabularyWordScreenOpens() =>
            Given(ApplicationStateWithVocabulary, MusicPace)
            .And(CurrentScreenIs, VocabularyBrowserScreen)
            .When(TheActionIs, OpenNewWordScreen())
            .Then(AssertTheCurrentScreenIs<ModifyVocabularyWordScreen>);

        [Fact]
        public void WhenNewVocabularyWordAdded_VocabularyBrowserScreenRestored()
            => Given(ApplicationStateWithVocabulary, MusicPace)
                .And(CurrentScreenIs, VocabularyBrowserScreen)
                .When(TheActionIs, OpenNewWordScreen())
                .And(TheActionIs, SaveNewVocabularyWord(Prestissimo))
                .Then(AssertTheCurrentScreenIs<VocabularyBrowserScreen>);
    }
}
