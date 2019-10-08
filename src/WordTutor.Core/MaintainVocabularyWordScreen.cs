using System;
using System.Collections.Generic;
using System.Text;

namespace WordTutor.Core
{
    public class ModifyVocabularyWordScreen : Screen, IEquatable<ModifyVocabularyWordScreen>
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
        /// Initializes a new instance of the <see cref="ModifyVocabularyWordScreen"/> class
        /// </summary>
        public ModifyVocabularyWordScreen()
        {
        }

        public ModifyVocabularyWordScreen WithSpelling(string spelling)
        {
            return new ModifyVocabularyWordScreen(
                this,
                spelling: spelling ?? string.Empty);
        }

        public ModifyVocabularyWordScreen WithPhrase(string phrase)
        {
            return new ModifyVocabularyWordScreen(
                this,
                phrase: phrase ?? string.Empty);
        }

        public ModifyVocabularyWordScreen WithPronunciation(string pronunciation)
        {
            return new ModifyVocabularyWordScreen(
                this,
                pronunciation: pronunciation ?? string.Empty);
        }

        public VocabularyWord AsWord()
        {
            return new VocabularyWord(Spelling)
                .WithPronunciation(Pronunciation)
                .WithPhrase(Phrase);
        }

        protected ModifyVocabularyWordScreen(
            ModifyVocabularyWordScreen original,
            string? spelling = null,
            string? pronunciation = null,
            string? phrase = null)
        {
            if (original is null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            Spelling = spelling ?? original.Spelling;
            Pronunciation = pronunciation ?? original.Pronunciation;
            Phrase = phrase ?? original.Phrase;
        }

        public override bool Equals(object? other) => Equals(other as ModifyVocabularyWordScreen);

        public override bool Equals(Screen? other) => Equals(other as ModifyVocabularyWordScreen);

        public override int GetHashCode()
        {
            unchecked
            {
                return Spelling.GetHashCode() * 23
                    + Pronunciation.GetHashCode() * 41
                    + Phrase.GetHashCode() * 71;
            }
        }

        public bool Equals(ModifyVocabularyWordScreen? other)
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
