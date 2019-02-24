using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace WordTutor.Core
{
    /// <summary>
    /// A set of words that make up a vocabulary for spelling drills
    /// </summary>
    public class VocabularySet
    {
        public static VocabularySet Empty = new VocabularySet();

        public ImmutableDictionary<string, VocabularyWord> Words { get; }

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

            if (Words.ContainsKey(word.Spelling))
            {
                throw new ArgumentException(
                    $"A word with spelling {word.Spelling} already exists in this set.",
                    nameof(word));
            }

            var words = Words.Add(word.Spelling, word);
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

            if (!Words.ContainsKey(word.Spelling))
            {
                throw new ArgumentException(
                    $"A word with spelling {word.Spelling} does not exist in this set.",
                    nameof(word));
            }

            var words = Words.Remove(word.Spelling);
            return new VocabularySet(this, words: words);
        }

        /// <summary>
        /// Replace one word with another
        /// </summary>
        /// <param name="existing">Word to be removed.</param>
        /// <param name="replacement">Replacement word to use instead.</param>
        /// <returns>A new vocabulary set wtih the specified replacement.</returns>
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

            if (!Words.ContainsKey(existing.Spelling))
            {
                throw new ArgumentException(
                    $"A word with spelling {existing.Spelling} does not exist in this set.",
                    nameof(existing));
            }

            if (existing.Equals(replacement))
            {
                return this;
            }

            var words = Words.Remove(existing.Spelling)
                .Add(replacement.Spelling, replacement);

            return new VocabularySet(this, words: words);
        }

        /// <summary>
        /// Update a word already in this set
        /// </summary>
        /// <param name="word">Spelling of the word to update.</param>
        /// <param name="transform">Tranformation to apply to the word.</param>
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

            if (!Words.TryGetValue(word, out var existing))
            {
                throw new ArgumentException(
                    $"A word with spelling {word} does not exist in this set.",
                    nameof(word));
            }

            var replacement = transform(existing);
            return Replace(existing, replacement);
        }

        private VocabularySet()
        {
            Words = ImmutableDictionary<string, VocabularyWord>.Empty;
        }

        private VocabularySet(
            VocabularySet original,
            ImmutableDictionary<string, VocabularyWord> words = null)
        {
            Words = words ?? original.Words;
        }
    }
}
