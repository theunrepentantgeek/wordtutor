using System;
using System.Collections.Generic;

namespace Niche.Common
{
    /// <summary>
    /// Successful validation
    /// </summary>
    public sealed class SuccessResult : ValidationResult, IEquatable<SuccessResult>
    {
        public override IEnumerable<ErrorResult> Errors() => _emptyErrors;

        public override IEnumerable<WarningResult> Warnings() => _emptyWarnings;

        public override bool HasErrors => false;

        public override bool HasWarnings => false;

        protected override ValidationResult Add(ValidationResult right)
            => right;

        public bool Equals(SuccessResult other)
            => other != null;

        public override bool Equals(ValidationResult other)
            => other is SuccessResult s
               && Equals(s);

        public override int GetHashCode()
            => typeof(SuccessResult).GetHashCode();
    }
}
