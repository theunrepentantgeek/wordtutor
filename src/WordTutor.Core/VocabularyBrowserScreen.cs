using System;

namespace WordTutor.Core
{
    public class VocabularyBrowserScreen : Screen
    {
        /// <summary>
        /// Gets the currently selected word
        /// </summary>
        public VocabularyWord Selection { get; }

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

        private VocabularyBrowserScreen(
            VocabularyBrowserScreen original,
            VocabularyWord selection = null,
            bool? modified = null)
        {
            Selection = selection ?? original.Selection;
            Modified = modified ?? original.Modified;
        }
    }
}
