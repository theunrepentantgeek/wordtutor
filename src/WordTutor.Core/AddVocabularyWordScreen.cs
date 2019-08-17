using System;
using System.Collections.Generic;
using System.Text;

namespace WordTutor.Core
{
    public class AddVocabularyWordScreen : Screen, IEquatable<AddVocabularyWordScreen>
    {
        /// <summary>
        /// Gets the spelling currently shown on screen
        /// </summary>
        public string Spelling { get; } = string.Empty;

        /// <summary>
        /// Gets the pronunciation currently shown on screen
        /// </summary>
        public string Pronunciation { get; } = string.Empty;

        /// <summary>
        /// Gets the guide phrase currently shown on screen
        /// </summary>
        public string Phrase { get; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddVocabularyWordScreen"/> class
        /// </summary>
        public AddVocabularyWordScreen()
        {
        }

        public AddVocabularyWordScreen WithSpelling(string spelling)
        {
            return new AddVocabularyWordScreen(
                this,
                spelling: spelling ?? string.Empty);
        }

        public AddVocabularyWordScreen WithPhrase(string phrase)
        {
            return new AddVocabularyWordScreen(
                this,
                phrase: phrase ?? string.Empty);
        }

        public AddVocabularyWordScreen WithPronunciation(string pronunciation)
        {
            return new AddVocabularyWordScreen(
                this,
                pronunciation: pronunciation ?? string.Empty);
        }

        protected AddVocabularyWordScreen(
            AddVocabularyWordScreen original,
            string spelling = null,
            string pronunciation = null,
            string phrase = null)
        {
            if (original is null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            Spelling = spelling ?? original.Spelling;
            Pronunciation = pronunciation ?? original.Pronunciation;
            Phrase = phrase ?? original.Phrase;
        }

        public override bool Equals(object other) 
            => Equals(other as AddVocabularyWordScreen);

        public override int GetHashCode()
        {
            unchecked
            {
                return Spelling.GetHashCode() * 23
                    + Pronunciation.GetHashCode() * 41
                    + Phrase.GetHashCode() * 71;
            }
        }

        public bool Equals(AddVocabularyWordScreen other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Spelling, other.Spelling, StringComparison.Ordinal)
                && string.Equals(Pronunciation, other.Pronunciation, StringComparison.Ordinal)
                && string.Equals(Phrase, other.Phrase, StringComparison.Ordinal);
        }
    }
}
