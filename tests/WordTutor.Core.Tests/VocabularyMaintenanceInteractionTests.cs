using Xunit;

using static Niche.GherkinSyntax.GherkinFactory;

namespace WordTutor.Core.Tests
{
    public class VocabularyMaintenanceInteractionTests : InteractionTestsBase
    {
        [Fact]
        public void WhenStartingNewVocabularyWord_AddVocabularyWordScreenOpens() =>
            Given(ApplicationStateWithVocabulary, MusicPace)
            .And(CurrentScreenIs, VocabularyBrowserScreen)
            .When(TheActionIs, OpenNewWordScreen())
            .Then(AssertTheCurrentScreenIs<AddVocabularyWordScreen>);
    }
}
