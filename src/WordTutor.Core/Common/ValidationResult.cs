using System;
using System.Collections.Generic;

namespace WordTutor.Core.Common
{
    /// <summary>
    /// Represents one or more messages that result from a validation check
    /// </summary>
    public abstract class ValidationResult : IEquatable<ValidationResult>
    {
        // an empty sequence of errors, cached to reduce allocations
        protected static readonly IEnumerable<ErrorResult> _emptyErrors 
            = new List<ErrorResult>();

        // an empty sequence of warnings, cached to reduce allocations
        protected static readonly IEnumerable<WarningResult> _emptyWarnings 
            = new List<WarningResult>();

        // Standardise string comparisons
        protected static readonly StringComparer _comparer = StringComparer.OrdinalIgnoreCase;

        /// <summary>
        /// Gets a (possibly empty) sequence of errors
        /// </summary>
        public abstract IEnumerable<ErrorResult> Errors();

        /// <summary>
        /// Gets a (possibly empty) sequence of warnings
        /// </summary>
        public abstract IEnumerable<WarningResult> Warnings();

        /// <summary>
        /// Gets a value indicating whether this result includes any errors
        /// </summary>
        public abstract bool HasErrors { get; }

        /// <summary>
        /// Gets a value indicating whether this results includes any warnings
        /// </summary>
        public abstract bool HasWarnings { get; }

        /// <summary>
        /// Combine two validation results into one
        /// </summary>
        public static ValidationResult operator +(ValidationResult left, ValidationResult right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            return left.Add(right ?? throw new ArgumentNullException(nameof(right)));
        }

        /// <summary>
        /// Combine two validation results into one
        /// </summary>
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
