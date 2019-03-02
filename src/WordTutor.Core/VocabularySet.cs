using System;
using System.Collections.Immutable;
using System.Linq;

namespace WordTutor.Core
{
    /// <summary>
    /// A set of words that make up a vocabulary for spelling drills
    /// </summary>
    public class VocabularySet //: IEquatable<VocabularySet>
    {
        private readonly ImmutableHashSet<VocabularyWord> _words;

        /// <summary>
        /// Creates an empty vocabulary set
        /// </summary>
        public static VocabularySet Empty { get; } = new VocabularySet();

        /// <summary>
        /// Gets a descriptive name for this set of words
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets all the words contained by this set
        /// </summary>
        public IImmutableSet<VocabularyWord> Words => _words;

        /// <summary>
        /// Change the name of the <see cref="VocabularySet"/>
        /// </summary>
        /// <param name="name">New descriptive name for the set.</param>
        /// <returns>A new vocabulary set with the requested name</returns>
        public VocabularySet WithName(string name)
        {
            if (string.Equals(name, Name))
            {
                return this;
            }

            return new VocabularySet(
                            this,
                            name: name ?? throw new ArgumentNullException(nameof(name)));
        }

        /// <summary>
        /// Add a new word into this set
        /// </summary>
        /// <param name="word">The word to add into the set.</param>
        /// <returns>A new vocabulary set that includes this word.</returns>
        public VocabularySet Add(VocabularyWord word)
        {
            if (word == null)
            {
                throw new ArgumentNullException(nameof(word));
            }

            if (_words.Contains(word))
            {
                return this;
            }

            if (_words.Any(w => w.HasSpelling(word.Spelling)))
            {
                throw new ArgumentException(
                    $"A word with spelling {word.Spelling} already exists in this set.",
                    nameof(word));
            }

            var words = _words.Add(word);
            return new VocabularySet(this, words: words);
        }

        /// <summary>
        /// Remove a word from this set
        /// </summary>
        /// <param name="word">The word to remove from the set.</param>
        /// <returns>A new vocabulary set without this word.</returns>
        public VocabularySet Remove(VocabularyWord word)
        {
            if (word == null)
            {
                throw new ArgumentNullException(nameof(word));
            }

            if (!Words.Contains(word))
            {
                throw new ArgumentException(
                    $"The word with spelling {word.Spelling} does not exist in this set.",
                    nameof(word));
            }

            var words = _words.Remove(word);
            return new VocabularySet(this, words: words);
        }

        /// <summary>
        /// Replace one word with another
        /// </summary>
        /// <param name="existing">Word to be removed.</param>
        /// <param name="replacement">Replacement word to use instead.</param>
        /// <returns>A new vocabulary set with the specified replacement.</returns>
        public VocabularySet Replace(VocabularyWord existing, VocabularyWord replacement)
        {
            if (existing == null)
            {
                throw new ArgumentNullException(nameof(existing));
            }

            if (replacement == null)
            {
                throw new ArgumentNullException(nameof(replacement));
            }

            if (!Words.Contains(existing))
            {
                throw new ArgumentException(
                    $"A word with spelling {existing.Spelling} does not exist in this set.",
                    nameof(existing));
            }

            if (existing.Equals(replacement))
            {
                return this;
            }

            var words = _words.Remove(existing)
                .Add(replacement);

            return new VocabularySet(this, words: words);
        }

        /// <summary>
        /// Update a word already in this set
        /// </summary>
        /// <param name="word">Spelling of the word to update.</param>
        /// <param name="transform">Transformation to apply to the word.</param>
        /// <returns></returns>
        public VocabularySet Update(string word, Func<VocabularyWord, VocabularyWord> transform)
        {
            if (word is null)
            {
                throw new ArgumentNullException(nameof(word));
            }

            if (transform is null)
            {
                throw new ArgumentNullException(nameof(transform));
            }

            var original = Words.FirstOrDefault(w => w.HasSpelling(word));
            if (original is null)
            {
                throw new ArgumentException(
                    $"A word with spelling {word} does not exist in this set.",
                    nameof(word));
            }

            var replacement = transform(original);
            return Replace(original, replacement);
        }

        private VocabularySet()
        {
            Name = string.Empty;
            _words = ImmutableHashSet<VocabularyWord>.Empty;
        }

        private VocabularySet(
            VocabularySet original,
            string name = null,
            ImmutableHashSet<VocabularyWord> words = null)
        {
            Name = name ?? original.Name;
            _words = words ?? original._words;
        }
    }
}
