using System;

namespace WordTutor.Core
{
    public class VocabularyWord : IEquatable<VocabularyWord>
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

        public bool Equals(VocabularyWord word)
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

        public override bool Equals(object obj)
            => Equals(obj as VocabularyWord);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Spelling.GetHashCode();
                hash = hash * 23 + Phrase.GetHashCode();
                hash = hash * 23 + Pronunciation.GetHashCode();
                return hash;
            }
        }

        private VocabularyWord(
            VocabularyWord original,
            string spelling = null,
            string pronunciation = null,
            string phrase = null)
        {
            Spelling = spelling ?? original.Spelling;
            Pronunciation = pronunciation ?? original.Pronunciation;
            Phrase = phrase ?? original.Phrase;
        }
    }
}
