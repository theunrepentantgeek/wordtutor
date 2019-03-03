using System.Collections.Generic;

using FluentAssertions;
using Xunit;

namespace WordTutor.Core.Common.Tests
{
    public class ValidationResultTests
    {
        public class HasErrors : ValidationResultTests
        {
            [Fact]
            public void GivenError_ReturnsTrue()
            {
                Validation.Error("Sample error").HasErrors.Should().BeTrue();
            }

            [Fact]
            public void GivenWarning_ReturnsFalse()
            {
                Validation.Warning("Sample error").HasErrors.Should().BeFalse();
            }

            [Fact]
            public void GivenSuccess_ReturnsFalse()
            {
                Validation.Success().HasErrors.Should().BeFalse();
            }
        }

        public class HasWarnings : ValidationResultTests
        {
            [Fact]
            public void GivenError_ReturnsFalse()
            {
                Validation.Error("Sample error").HasWarnings.Should().BeFalse();
            }

            [Fact]
            public void GivenWarning_ReturnsTrue()
            {
                Validation.Warning("Sample warning").HasWarnings.Should().BeTrue();
            }

            [Fact]
            public void GivenSuccess_ReturnsFalse()
            {
                Validation.Success().HasWarnings.Should().BeFalse();
            }
        }

        public class Errors : ValidationResultTests
        {
            [Fact]
            public void GivenError_ReturnsExpectedCount()
            {
                Validation.Error("Sample error").Errors.Should().HaveCount(1);
            }

            [Fact]
            public void GivenSuccess_ReturnsExpectedCount()
            {
                Validation.Success().Errors.Should().BeEmpty();
            }
        }

        public class OperatorTests : ValidationResultTests
        {
            [Theory]
            [MemberData(nameof(EagerTestCases))]
            public void PlusTests(ValidationResult left, ValidationResult right, ValidationResult expected)
                => (left + right).Should().Be(expected);

            [Theory]
            [MemberData(nameof(EagerTestCases))]
            public void AndTests(ValidationResult left, ValidationResult right, ValidationResult expected)
                => (left & right).Should().Be(expected);

            [Theory]
            [MemberData(nameof(LazyTestCases))]
            public void LazyAndTests(ValidationResult left, ValidationResult right, ValidationResult expected)
                => (left && right).Should().Be(expected);

            public static IEnumerable<object[]> EagerTestCases()
            {
                var success = Validation.Success();
                var warning = Validation.Warning("warning");
                var otherWarning = Validation.Warning("other warning");
                var error = Validation.Error("error");
                var otherError = Validation.Error("other error");

                var aggregateWE = new AggregateResult(warning, error);
                var aggregateWW = new AggregateResult(warning, otherWarning);
                var aggregateEE = new AggregateResult(error, otherError);

                yield return new object[] { success, success, success };

                yield return new object[] { success, warning, warning };
                yield return new object[] { warning, success, warning };

                yield return new object[] { success, error, error };
                yield return new object[] { error, success, error };

                yield return new object[] { warning, error, aggregateWE };
                yield return new object[] { error, warning, aggregateWE };

                yield return new object[] { success, aggregateWE, aggregateWE };
                yield return new object[] { aggregateWE, success, aggregateWE };

                yield return new object[] { aggregateWE, warning, aggregateWE };
                yield return new object[] { warning, aggregateWE, aggregateWE };

                yield return new object[] { aggregateWE, error, aggregateWE };
                yield return new object[] { error, aggregateWE, aggregateWE };

                yield return new object[] { warning, otherWarning, aggregateWW };
                yield return new object[] { error, otherError, aggregateEE };
            }

            public static IEnumerable<object[]> LazyTestCases()
            {
                var success = Validation.Success();
                var warning = Validation.Warning("warning");
                var error = Validation.Error("error");

                var aggregate = new AggregateResult(warning, error);

                yield return new object[] {success, success, success};

                yield return new object[] {success, warning, warning};
                yield return new object[] {warning, success, warning};

                yield return new object[] {success, error, error};
                yield return new object[] {error, success, error};

                yield return new object[] { success, aggregate, aggregate };
                yield return new object[] { aggregate, success, aggregate };

                yield return new object[] {warning, error, aggregate};
                yield return new object[] {error, warning, error};

                yield return new object[] { warning, aggregate, aggregate };
                yield return new object[] { aggregate, warning, aggregate };

                yield return new object[] { error, aggregate, error };
                yield return new object[] { error, warning, error };
            }
        }
    }
}
