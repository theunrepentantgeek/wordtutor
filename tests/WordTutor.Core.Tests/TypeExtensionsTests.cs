using FluentAssertions;
using System;
using System.Collections.Immutable;
using System.Text;
using Xunit;

namespace WordTutor.Core.Tests
{
    public class TypeExtensionsTests
    {
        public class IsImmutableType: TypeExtensionsTests
        {
            [Theory]
            [InlineData(typeof(VocabularyWord), true)]
            [InlineData(typeof(string), true)]
            [InlineData(typeof(StringBuilder), false)]
            [InlineData(typeof(IImmutableSet<VocabularyWord>), true)]
            [InlineData(typeof(IImmutableSet<StringBuilder>), false)]
            public void GivenType_ReturnsExpectedResult(Type type, bool isImmutable)
            {
                var modifier = isImmutable ? "" : "not ";
                type.IsImmutableType()
                    .Should().Be(
                        isImmutable,
                        $"Type {type.Name} should {modifier} be immutable.");
            }
        }
    }
}
