using System;

namespace WordTutor.Core.Common
{
    /// <summary>
    /// A reusable piece of metadata that can be associated with a <see cref="ValidationResult"/>
    /// </summary>
    public class ValidationMetadata : IEquatable<ValidationMetadata>
    {
        private static readonly StringComparer _comparer =
            StringComparer.OrdinalIgnoreCase;

        public string Name { get; }
        public object Value { get; }

        public ValidationMetadata(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Value = value;
        }

        public bool Equals(ValidationMetadata other)
            => _comparer.Equals(Name, other?.Name)
               && Equals(Value, other?.Value);

        public override bool Equals(object obj)
            => obj is ValidationMetadata vm
               && Equals(vm);

        public override int GetHashCode()
            => Name.GetHashCode() ^ Value.GetHashCode();

        public override string ToString() => $"{Name}: {Value}";
    }
}
