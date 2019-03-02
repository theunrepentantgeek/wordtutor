using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace WordTutor.Core.Common
{
    public sealed class WarningResult : ValidationResultWithMetadata, IEquatable<WarningResult>
    {
        public string Message { get; }

        public WarningResult(string message, IEnumerable<ValidationMetadata> metadata)
            : base(metadata)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public override IEnumerable<ErrorResult> Errors() => _emptyErrors;

        public override IEnumerable<WarningResult> Warnings()
        {
            yield return this;
        }

        public override bool HasErrors => false;

        public override bool HasWarnings => true;

        public bool Equals(WarningResult other)
            => other != null
               && _comparer.Equals(Message, other.Message);

        public override bool Equals(ValidationResult other)
            => other is WarningResult w
               && Equals(w);

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
    }
}
