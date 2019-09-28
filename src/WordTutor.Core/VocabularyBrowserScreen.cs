using System;

namespace WordTutor.Core
{
    public class VocabularyBrowserScreen : Screen, IEquatable<VocabularyBrowserScreen>
    {
        /// <summary>
        /// Gets the currently selected word (may be null)
        /// </summary>
        public VocabularyWord? Selection { get; }

        /// <summary>
        /// Gets a value indicating whether we have unsaved changes
        /// </summary>
        public bool Modified { get; }

        public VocabularyBrowserScreen()
        {
        }

        public VocabularyBrowserScreen WithSelection(VocabularyWord word)
        {
            if (Equals(Selection, word ?? throw new ArgumentNullException(nameof(word))))
            {
                return this;
            }

            return new VocabularyBrowserScreen(
                this,
                selection: word);
        }

        public VocabularyBrowserScreen WithNoSelection()
        {
            if (Selection is null)
            {
                return this;
            }

            return new VocabularyBrowserScreen(this, selection: null);
        }

        public VocabularyBrowserScreen MarkAsModified()
        {
            if (Modified)
            {
                return this;
            }

            return new VocabularyBrowserScreen(
                this,
                modified: true);
        }

        public VocabularyBrowserScreen MarkAsUnmodified()
        {
            if (!Modified)
            {
                return this;
            }

            return new VocabularyBrowserScreen(
                this,
                modified: false);
        }

        public override bool Equals(object? obj) => Equals(obj as VocabularyBrowserScreen);

        public override bool Equals(Screen? other) => Equals(other as VocabularyBrowserScreen);

        public bool Equals(VocabularyBrowserScreen? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Selection.Equals(other.Selection)
                && Modified == other.Modified;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Selection.GetHashCode() * 23
                    + Modified.GetHashCode();
            }
        }

        private VocabularyBrowserScreen(
            VocabularyBrowserScreen original,
            VocabularyWord? selection = null,
            bool? modified = null)
        {
            Selection = selection ?? original.Selection;
            Modified = modified ?? original.Modified;
        }
    }
}
