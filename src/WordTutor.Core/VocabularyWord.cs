using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace WordTutor.Core
{
    // If using a VocabularyWord? for a T where T : IEquatable<T>
    // then VW needs to implement IEquatable<VW?> 
    // in order to satisfy the expanded type constraint IEquatable<VW?>

    [DebuggerDisplay("Word: '{Spelling}'")]
    public class VocabularyWord : IEquatable<VocabularyWord?>
    {
        /// <summary>
        /// Gets the word as correctly spelt
        /// </summary>
        public string Spelling { get; }

        /// <summary>
        /// Gets the word as said 
        /// </summary>
        /// <remarks>
        /// This allows pronunciation to be customized if the voice says the word in an unexpected way.
        /// </remarks>
        public string Pronunciation { get; }

        /// <summary>
        /// Gets a phrase using the word in context
        /// </summary>
        public string Phrase { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VocabularyWord"/> class
        /// </summary>
        /// <param name="spelling">
        /// Correct spelling for the word
        /// </param>
        public VocabularyWord(string spelling)
        {
            Spelling = spelling ?? throw new ArgumentNullException(nameof(spelling));
            Pronunciation = string.Empty;
            Phrase = string.Empty;
        }

        /// <summary>
        /// Specify a new spelling for this word
        /// </summary>
        /// <param name="spelling">New correct spelling.</param>
        /// <returns>A new instance with modified spelling.</returns>
        public VocabularyWord WithSpelling(string spelling)
        {
            return new VocabularyWord(
                this,
                spelling: spelling ?? throw new ArgumentNullException(nameof(spelling)));
        }

        /// <summary>
        /// Check to see if this word has the specified spelling
        /// </summary>
        /// <param name="spelling">Spelling we're interested in finding.</param>
        /// <returns>True if the spelling matches (case insensitive), false otherwise.</returns>
        public bool HasSpelling(string spelling)
            => string.Equals(
                spelling ?? throw new ArgumentNullException(nameof(spelling)),
                Spelling,
                StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        /// Specify a new pronunciation for this word
        /// </summary>
        /// <param name="pronunciation">New pronunciation.</param>
        /// <returns>A new instance with modified pronunciation.</returns>
        public VocabularyWord WithPronunciation(string pronunciation)
        {
            return new VocabularyWord(
                this,
                pronunciation: pronunciation ?? throw new ArgumentNullException(nameof(pronunciation)));
        }

        /// <summary>
        /// Specify a new sample phrase for this word
        /// </summary>
        /// <param name="phrase">New sample phrase.</param>
        /// <returns>A new instance with modified sample phrase.</returns>
        public VocabularyWord WithPhrase(string phrase)
        {
            return new VocabularyWord(
                this,
                phrase: phrase ?? throw new ArgumentNullException(nameof(phrase)));
        }

        [SuppressMessage(
            "Maintainability",
            "RCS1168:Parameter name differs from base name.",
            Justification = "The name 'word' better identifies the passed value")]
        public bool Equals(VocabularyWord? word)
        {
            if (word is null)
            {
                return false;
            }

            if (ReferenceEquals(this, word))
            {
                return true;
            }

            return Equals(Spelling, word.Spelling)
                && Equals(Phrase, word.Phrase)
                && Equals(Pronunciation, word.Pronunciation);
        }

        public override bool Equals(object? obj)
            => Equals(obj as VocabularyWord);

        public override int GetHashCode()
            => HashCode.Combine(Spelling, Phrase, Pronunciation);

        private VocabularyWord(
            VocabularyWord original,
            string? spelling = null,
            string? pronunciation = null,
            string? phrase = null)
        {
            Spelling = spelling ?? original.Spelling;
            Pronunciation = pronunciation ?? original.Pronunciation;
            Phrase = phrase ?? original.Phrase;
        }
    }
}
