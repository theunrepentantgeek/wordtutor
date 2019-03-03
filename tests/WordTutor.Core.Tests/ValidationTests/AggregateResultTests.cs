using System;
using System.Collections.Immutable;
using System.Linq;

using FluentAssertions;
using Xunit;

namespace WordTutor.Core.Common.Tests
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
                aggregate.Errors.Should().HaveCount(1);
                aggregate.Errors.Single().Should().Be(_error);
            }

            [Fact]
            public void GivenSameErrorMultipleTimes_HasSingleError()
            {
                var aggregate = new AggregateResult(_error, _error, _error);
                aggregate.Errors.Should().HaveCount(1);
                aggregate.Errors.Single().Should().Be(_error);
            }

            [Fact]
            public void GivenEquivilentErrors_HasSingleError()
            {
                var aggregate = new AggregateResult(_error, _sameError, _error);
                aggregate.Errors.Should().HaveCount(1);
            }
        }

        public class AdditionOperator : AggregateResultTests
        {
            private readonly AggregateResult _emptyAggregate = new AggregateResult();
            private readonly ValidationResult _passwordError = Validation.Error("No password");
            private readonly ValidationResult _passwordWarning = Validation.Warning("Password insecure (too short)");

            [Fact]
            public void WithError_HasErrors()
            {
                var vr = _emptyAggregate + _passwordError;
                vr.HasErrors.Should().BeTrue();
            }

            [Fact]
            public void WithWarning_HasWarnings()
            {
                var vr = _emptyAggregate + _passwordWarning;
                vr.HasWarnings.Should().BeTrue();
            }

            [Fact]
            public void WithAggregate_HasCombinedErrors()
            {
                var ag = new AggregateResult(_passwordError, _passwordWarning)
                    + _aggregate;
                ag.Errors.Should().Contain(ag.Errors);
                ag.Errors.Should().Contain(_aggregate.Errors);
            }

            [Fact]
            public void WithAggregate_HasCombinedWarnings()
            {
                var ag = new AggregateResult(_passwordError, _passwordWarning)
                    + _aggregate;
                ag.Warnings.Should().Contain(ag.Warnings);
                ag.Warnings.Should().Contain(_aggregate.Warnings);
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

        public class GetHashCodeMethod : AggregateResultTests
        {
            [Fact]
            public void ReturnsConsistentValue()
            {
                _aggregate.GetHashCode().Should().Be(_aggregate.GetHashCode());
            }

            [Fact]
            public void ForSameResults_ReturnsConsistentValue()
            {
                // Add in reverse order to _aggregate
                var ag = new AggregateResult(_warning, _error);
                ag.GetHashCode().Should().Be(_aggregate.GetHashCode());
            }

            [Fact]
            public void ForDifferentResults_ReturnsDifferentValue()
            {
                var ag = new AggregateResult(_warning);
                ag.GetHashCode().Should().NotBe(_aggregate.GetHashCode());
            }
        }
    }
}
