using System;
using System.Collections.Generic;

namespace Niche.Common
{
    /// <summary>
    /// Represents one or more messages that result from a validation check
    /// </summary>
    public abstract class ValidationResult : IEquatable<ValidationResult>
    {
        protected static readonly IEnumerable<ErrorResult> _emptyErrors = new List<ErrorResult>();

        protected static readonly IEnumerable<WarningResult> _emptyWarnings = new List<WarningResult>();

        protected static readonly StringComparer _comparer = StringComparer.OrdinalIgnoreCase;

        public abstract IEnumerable<ErrorResult> Errors();

        public abstract IEnumerable<WarningResult> Warnings();

        public abstract bool HasErrors { get; }

        public abstract bool HasWarnings { get; }

        public static ValidationResult operator +(ValidationResult left, ValidationResult right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            return left.Add(right ?? throw new ArgumentNullException(nameof(right)));
        }

        public static ValidationResult operator &(ValidationResult left, ValidationResult right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            return left.Add(right ?? throw new ArgumentNullException(nameof(right)));
        }

        public static bool operator true(ValidationResult result)
            => !result.HasErrors;

        public static bool operator false(ValidationResult result)
            => result.HasErrors;

        public static bool Equals(ValidationResult left, ValidationResult right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left == null || right == null)
            {
                return false;
            }

            return left.Equals(right);
        }

        public abstract bool Equals(ValidationResult other);

        protected abstract ValidationResult Add(ValidationResult right);

        // Prevent any implementations outside this assembly
        private protected ValidationResult()
        {
        }
    }
}
