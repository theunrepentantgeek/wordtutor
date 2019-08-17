namespace WordTutor.Core.Tests.Fakes
{
    /// <summary>
    /// A fake screen for use when testing
    /// </summary>
    public class FakeScreen : Screen
    {
        public override bool Equals(Screen other) => other is FakeScreen;
    }
}
