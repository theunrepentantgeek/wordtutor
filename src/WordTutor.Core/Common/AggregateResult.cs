using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace WordTutor.Core.Common
{
    public sealed class AggregateResult : ValidationResult, IEquatable<AggregateResult>
    {
        internal readonly ImmutableHashSet<ValidationResult> Results;

        public AggregateResult(ImmutableHashSet<ValidationResult> results)
        {
            Results = results ?? throw new ArgumentNullException(nameof(results));
        }

        public AggregateResult(params ValidationResult[] results)
            : this(results.ToImmutableHashSet())
        { }

        public override IEnumerable<ErrorResult> Errors()
        {
            return Results.OfType<ErrorResult>();
        }

        public override IEnumerable<WarningResult> Warnings()
        {
            return Results.OfType<WarningResult>();
        }

        public override bool HasErrors => Errors().Any();

        public override bool HasWarnings => Warnings().Any();

        public bool Equals(AggregateResult other)
            => other?.Results.SetEquals(Results) ?? false;

        public override bool Equals(ValidationResult other)
            => other is AggregateResult a
               && Equals(a);

        public override bool Equals(object obj)
            => obj is AggregateResult a
               && Equals(a);

        public override int GetHashCode()
            => Results.Aggregate(
                217_645_177,
                (hash, result) => hash ^ result.GetHashCode());

        protected override ValidationResult Add(ValidationResult right)
        {
            switch (right)
            {
                case SuccessResult _:
                    return this;

                case WarningResult w:
                    return new AggregateResult(
                        Results.Add(w));

                case ErrorResult e:
                    return new AggregateResult(
                        Results.Add(e));

                case AggregateResult a:
                    return new AggregateResult(
                        a.Results.Union(Results));

                default:
                    throw new InvalidOperationException("Unexpected subclass of ValidationResult found");
            }
        }
    }
}