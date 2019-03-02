using System.Collections.Generic;

using FluentAssertions;
using Xunit;

namespace WordTutor.Core.Common.Tests
{
    public class SuccessResultTests
    {
        private readonly SuccessResult _result = new SuccessResult();

        public class EqualsObject : SuccessResultTests
        {
            [Fact]
            public void GivenNull_ReturnsFalse()
            {
                object other = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                _result.Equals(other).Should().BeFalse();
            }

            [Fact]
            public void GivenSelf_ReturnsTrue()
            {
                object other = _result;
                _result.Equals(other).Should().BeTrue();
            }

            [Fact]
            public void GivenDifferent_ReturnsFalse()
            {
                object other = this;
                _result.Equals(other).Should().BeFalse();
            }
        }

        public class EqualsValidationResult : SuccessResultTests
        {
            [Fact]
            public void GivenNull_ReturnsFalse()
            {
                ValidationResult other = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                _result.Equals(other).Should().BeFalse();
            }

            [Fact]
            public void GivenSelf_ReturnsTrue()
            {
                ValidationResult other = _result;
                _result.Equals(other).Should().BeTrue();
            }

            [Fact]
            public void GivenDifferent_ReturnsFalse()
            {
                ValidationResult other = new ErrorResult("error", new List<ValidationMetadata>());
                _result.Equals(other).Should().BeFalse();
            }
        }

        public class EqualsSuccessResult : SuccessResultTests
        {
            [Fact]
            public void GivenNull_ReturnsFalse()
            {
                SuccessResult other = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                _result.Equals(other).Should().BeFalse();
            }

            [Fact]
            public void GivenSelf_ReturnsTrue()
            {
                SuccessResult other = _result;
                _result.Equals(other).Should().BeTrue();
            }
        }
    }
}
