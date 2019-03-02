using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Niche.Common
{
    /// <summary>
    /// Base class for a single validation result
    /// </summary>
    public abstract class ValidationResultWithMetadata : ValidationResult
    {
        /// <summary>
        /// Additional metadata about this validation result
        /// </summary>
        public IReadOnlyDictionary<string, object> Metadata { get; }

        // Prevent any implementations outside this assembly
        private protected ValidationResultWithMetadata(IEnumerable<ValidationMetadata> metadata)
        {
            if (metadata is null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            Metadata = metadata.ToImmutableDictionary(
                m => m.Name,
                m => m.Value,
                StringComparer.OrdinalIgnoreCase);
        }
    }
}
