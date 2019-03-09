using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;

namespace WordTutor.Core
{
    public class WordTutorApplication
    {
        // a stack of active screens, with the topmost one visible to the user
        private readonly ImmutableStack<Screen> _screens;

        /// <summary>
        /// Gets the current vocabulary being edited
        /// </summary>
        public VocabularySet VocabularySet { get; }

        /// <summary>
        /// Gets the current screen available for user interaction
        /// </summary>
        public Screen CurrentScreen => _screens.Peek();

        /// <summary>
        /// Initializes a new instance of the <see cref="WordtutorApplication"/> class
        /// </summary>
        /// <param name="initialScreen"></param>
        public WordTutorApplication(Screen initialScreen)
        {
            VocabularySet = VocabularySet.Empty;
            _screens = ImmutableStack<Screen>.Empty
                .Push(initialScreen ?? throw new ArgumentNullException(nameof(initialScreen)));
        }

        /// <summary>
        /// Open a new screen, adding it to our stack
        /// </summary>
        /// <param name="screen">New screen to open.</param>
        public WordTutorApplication OpenScreen(Screen screen)
        {
            var screens = _screens.Push(
                screen ?? throw new ArgumentNullException(nameof(screen)));
            return new WordTutorApplication(this, screens: screens);
        }

        /// <summary>
        /// Closes the top screen on our stack, revealing the screen underneath
        /// </summary>
        /// <remarks>
        /// Won't close the top screen.
        /// </remarks>
        public WordTutorApplication CloseScreen()
        {
            if (_screens.Pop().IsEmpty)
            {
                // Don't ever want to close the last screen
                return this;
            }

            var screens = _screens.Pop();
            return new WordTutorApplication(this, screens: screens);
        }

        /// <summary>
        /// Modifies the current screen
        /// </summary>
        /// <remarks>
        /// If the screen is not changed, the existing instance will be returned
        /// </remarks>
        /// <param name="transformation">Transformation to apply to the screen</param>
        public WordTutorApplication UpdateScreen(Func<Screen, Screen> transformation)
        {
            if (transformation is null)
            {
                throw new ArgumentNullException(nameof(transformation));
            }

            var screen = transformation(CurrentScreen);
            if (ReferenceEquals(screen, CurrentScreen))
            {
                return this;
            }

            var screens = _screens.Pop().Push(screen);
            return new WordTutorApplication(this, screens: screens);
        }

        private WordTutorApplication(
            WordTutorApplication original,
            VocabularySet vocabularySet = null,
            ImmutableStack<Screen> screens = null)
        {
            VocabularySet = vocabularySet ?? original.VocabularySet;
            _screens = screens ?? original._screens;
        }
    }
}
