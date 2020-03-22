using FluentAssertions;
using System;
using Xunit;

namespace WordTutor.Core.Tests
{
    public class CachedTests
    {
        private int _generated;

        [Fact]
        public void Constructor_WhenGivenNullGenerator_ThrowsExpectedException()
        {
            var exception =
                Assert.Throws<ArgumentNullException>(
                    () => new Cached<string>(generator: null!));
            exception.Should().BeOfType<ArgumentNullException>();
            exception.ParamName.Should().Be("generator");
        }

        [Fact]
        public void Value_WhenReadForTheFirstTime_ReturnsExpectedValue()
        {
            var c = new Cached<string>(Generator);
            c.Value.Should().Be("result#1");
        }

        [Fact]
        public void Value_WhenReadManyTimes_ReturnsExpectedValue()
        {
            var c = new Cached<string>(Generator);
            for (int i = 0; i < 100; i++)
            {
                c.Value.Should().Be("result#1");
            }
        }

        [Fact]
        public void Value_WhenReadAfterClear_ReturnsExpectedValue()
        {
            var c = new Cached<string>(Generator);
            var prior = c.Value;
            c.Clear();
            var current = c.Value;
            current.Should().Be("result#2");
            current.Should().NotBe(prior);
        }

        private string Generator()
        {
            _generated++;
            return $"result#{_generated}";
        }
    }
}
