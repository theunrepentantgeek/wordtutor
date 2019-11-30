using System;
using System.Collections.Generic;

namespace WordTutor.Core
{
    [Immutable]
    public sealed class ModifyVocabularyWordScreen : Screen, IEquatable<ModifyVocabularyWordScreen?>
    {
        private readonly static EqualityComparer<VocabularyWord> _wordComparer
            = EqualityComparer<VocabularyWord>.Default;

        /// <summary>
        /// Gets the existing word we're modifying (if any)
        /// </summary>
        /// <remarks>
        /// Will be empty if we're creating a new word.
        /// </remarks>
        public VocabularyWord? ExistingWord { get; }

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
        /// Gets a value indicating whether the word has been modified
        /// </summary>
        public bool Modified { get; }

        public static ModifyVocabularyWordScreen ForNewWord()
            => new ModifyVocabularyWordScreen();

        public static ModifyVocabularyWordScreen ForExistingWord(
            VocabularyWord word)
            => new ModifyVocabularyWordScreen(word);

        public ModifyVocabularyWordScreen WithSpelling(string spelling)
            => new ModifyVocabularyWordScreen(
                this,
                spelling: spelling ?? string.Empty,
                modified: true);

        public ModifyVocabularyWordScreen WithPhrase(string phrase)
            => new ModifyVocabularyWordScreen(
                this,
                phrase: phrase ?? string.Empty,
                modified: true);

        public ModifyVocabularyWordScreen WithPronunciation(string pronunciation)
            => new ModifyVocabularyWordScreen(
                this,
                pronunciation: pronunciation ?? string.Empty,
                modified: true);

        public ModifyVocabularyWordScreen ClearModified()
            => new ModifyVocabularyWordScreen(this, modified: false);

        public VocabularyWord AsWord()
        {
            return new VocabularyWord(Spelling)
                .WithPronunciation(Pronunciation)
                .WithPhrase(Phrase);
        }

        private ModifyVocabularyWordScreen(VocabularyWord? existingWord = null)
        {
            ExistingWord = existingWord;
            Spelling = existingWord?.Spelling ?? string.Empty;
            Pronunciation = existingWord?.Pronunciation ?? string.Empty;
            Phrase = existingWord?.Phrase ?? string.Empty;
            Modified = false;
        }

        private ModifyVocabularyWordScreen(
            ModifyVocabularyWordScreen original,
            string? spelling = null,
            string? pronunciation = null,
            string? phrase = null,
            bool? modified = null)
        {
            if (original is null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            ExistingWord = original.ExistingWord;
            Spelling = spelling ?? original.Spelling;
            Pronunciation = pronunciation ?? original.Pronunciation;
            Phrase = phrase ?? original.Phrase;
            Modified = modified ?? original.Modified;
        }

        public override bool Equals(object? other)
            => Equals(other as ModifyVocabularyWordScreen);

        public override bool Equals(Screen? other)
            => Equals(other as ModifyVocabularyWordScreen);

        public override int GetHashCode()
            => HashCode.Combine(Spelling, Pronunciation, Phrase);

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
                && string.Equals(Phrase, other.Phrase, StringComparison.Ordinal)
                && _wordComparer.Equals(ExistingWord, other.ExistingWord);
        }
    }
}
