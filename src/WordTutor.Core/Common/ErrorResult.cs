using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace WordTutor.Core.Common
{
    /// <summary>
    /// A single validation error
    /// </summary>
    public sealed class ErrorResult : ValidationResultWithMetadata, IEquatable<ErrorResult>
    {
        public string Message { get; }

        public ErrorResult(string message, IEnumerable<ValidationMetadata> metadata)
            : base(metadata)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public override IEnumerable<ErrorResult> Errors()
        {
            yield return this;
        }

        public override IEnumerable<WarningResult> Warnings() => _emptyWarnings;

        public override bool HasErrors => true;

        public override bool HasWarnings => false;

        public bool Equals(ErrorResult other)
            => other != null
               && _comparer.Equals(Message, other.Message);

        public override bool Equals(ValidationResult other)
            => other is ErrorResult e
               && Equals(e);

        protected override ValidationResult Add(ValidationResult right)
        {
            switch (right)
            {
                case SuccessResult _:
                    return this;

                case WarningResult w:
                    return new AggregateResult(
                        ImmutableHashSet<ValidationResult>.Empty
                            .Add(this)
                            .Add(w));

                case ErrorResult e:
                    return new AggregateResult(
                        ImmutableHashSet<ValidationResult>.Empty
                            .Add(this)
                            .Add(e));

                case AggregateResult a:
                    return new AggregateResult(
                        a.Results.Add(this));

                default:
                    throw new InvalidOperationException("Unexpected subclass of ValidationResult found");
            }
        }

        public override int GetHashCode() => _comparer.GetHashCode(Message);
    }
}
