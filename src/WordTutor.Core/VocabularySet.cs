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

        private VocabularySet()
        {
            Words = ImmutableDictionary<string, VocabularyWord>.Empty;
        }

        private VocabularySet(
            VocabularySet original,
            ImmutableDictionary<string,VocabularyWord> words = null)
        {
            Words = words ?? original.Words;
        }
    }

}
