using System;
using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Niche.Common.Tests
{
    public class AggregateResultTests
    {
        private readonly ValidationResult _error = Validation.Error("An Error");
        private readonly ValidationResult _sameError = Validation.Error("An Error");
        private readonly ValidationResult _warning = Validation.Warning("A warning");

        private readonly AggregateResult _aggregate;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public AggregateResultTests()
        {
            var results = ImmutableHashSet<ValidationResult>.Empty
                .Add(_error)
                .Add(_warning);
            _aggregate = new AggregateResult(results);
        }

        public class ConstructorHashSet : AggregateResultTests
        {
            [Fact]
            public void GivenEmptySet_ReturnsInstanceWithNoErrors()
            {
                var result = new AggregateResult(ImmutableHashSet<ValidationResult>.Empty);
                result.HasErrors.Should().BeFalse();
            }

            [Fact]
            public void GivenEmptySet_ReturnsInstanceWithNoWarnings()
            {
                var result = new AggregateResult(ImmutableHashSet<ValidationResult>.Empty);
                result.HasWarnings.Should().BeFalse();
            }

            [Fact]
            public void GivenSetWithAnError_ReturnsInstanceWithErrors()
            {
                var set = ImmutableHashSet<ValidationResult>.Empty
                    .Add(_error);
                var result = new AggregateResult(set);
                result.HasErrors.Should().BeTrue();
            }

            [Fact]
            public void GivenSetWithAnError_ReturnsInstanceWithoutWarnings()
            {
                var set = ImmutableHashSet<ValidationResult>.Empty
                    .Add(_error);
                var result = new AggregateResult(set);
                result.HasWarnings.Should().BeFalse();
            }

            [Fact]
            public void GivenSetWithAWarning_ReturnsInstanceWithoutErrors()
            {
                var set = ImmutableHashSet<ValidationResult>.Empty
                    .Add(_warning);
                var result = new AggregateResult(set);
                result.HasErrors.Should().BeFalse();
            }

            [Fact]
            public void GivenSetWithAWarning_ReturnsInstanceWithWarnings()
            {
                var set = ImmutableHashSet<ValidationResult>.Empty
                    .Add(_warning);
                var result = new AggregateResult(set);
                result.HasWarnings.Should().BeTrue();
            }

            [Fact]
            public void GivenNull_ThrowsExpectedException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () =>
                        {
                            var nullSet = (ImmutableHashSet<ValidationResult>)null;
                            // ReSharper disable once ExpressionIsAlwaysNull
                            return new AggregateResult(nullSet);
                        });
                exception.ParamName.Should().Be("results");
            }
        }

        public class ConstructorWithParams : AggregateResultTests
        {
            [Fact]
            public void GivenSingleError_HasSingleError()
            {
                var aggregate = new AggregateResult(_error);
                aggregate.Errors().Should().HaveCount(1);
                aggregate.Errors().Single().Should().Be(_error);
            }

            [Fact]
            public void GivenSameErrorMultipleTimes_HasSingleError()
            {
                var aggregate = new AggregateResult(_error, _error, _error);
                aggregate.Errors().Should().HaveCount(1);
                aggregate.Errors().Single().Should().Be(_error);
            }

            [Fact]
            public void GivenEquivilentErrors_HasSingleError()
            {
                var aggregate = new AggregateResult(_error, _sameError, _error);
                aggregate.Errors().Should().HaveCount(1);
            }
        }

        public class EqualsObject : AggregateResultTests
        {
            [Fact]
            public void EqualsNull_ReturnsFalse()
            {
                object other = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                _aggregate.Equals(other).Should().BeFalse();
            }

            [Fact]
            public void EqualsSelf_ReturnsTrue()
            {
                object other = _aggregate;
                _aggregate.Equals(other).Should().BeTrue();
            }

            [Fact]
            public void EqualsEquivilentError_ReturnsTrue()
            {
                var results = ImmutableHashSet<ValidationResult>.Empty
                    .Add(_error)
                    .Add(_warning);
                object other = new AggregateResult(results);
                _aggregate.Equals(other).Should().BeTrue();
            }

            [Fact]
            public void EqualsDifferentType_ReturnsFalse()
            {
                object other = this;
                _aggregate.Equals(other).Should().BeFalse();
            }
        }

        public class EqualsValidationResult : AggregateResultTests
        {
            [Fact]
            public void EqualsNull_ReturnsFalse()
            {
                ValidationResult other = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                _aggregate.Equals(other).Should().BeFalse();
            }

            [Fact]
            public void EqualsSelf_ReturnsTrue()
            {
                ValidationResult other = _aggregate;
                _aggregate.Equals(other).Should().BeTrue();
            }

            [Fact]
            public void EqualsEquivilentError_ReturnsTrue()
            {
                var results = ImmutableHashSet<ValidationResult>.Empty
                    .Add(_error)
                    .Add(_warning);
                ValidationResult other = new AggregateResult(results);
                _aggregate.Equals(other).Should().BeTrue();
            }
        }

        public class EqualsAggregateResult : AggregateResultTests
        {
            [Fact]
            public void EqualsNull_ReturnsFalse()
            {
                AggregateResult other = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                _aggregate.Equals(other).Should().BeFalse();
            }

            [Fact]
            public void EqualsSelf_ReturnsTrue()
            {
                AggregateResult other = _aggregate;
                _aggregate.Equals(other).Should().BeTrue();
            }

            [Fact]
            public void EqualsEquivilentAggregate_ReturnsTrue()
            {
                var results = ImmutableHashSet<ValidationResult>.Empty
                    .Add(_error)
                    .Add(_warning);
                AggregateResult other = new AggregateResult(results);
                _aggregate.Equals(other).Should().BeTrue();
            }
        }
    }
}
