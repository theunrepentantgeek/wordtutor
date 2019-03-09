using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;

namespace WordTutor.Core
{
    public class WordTutorApplication
    {
        // a stack of previously active screens
        private readonly ImmutableStack<Screen> _priorScreens;

        /// <summary>
        /// Gets the current vocabulary being edited
        /// </summary>
        public VocabularySet VocabularySet { get; }

        /// <summary>
        /// Gets the current screen available for user interaction
        /// </summary>
        public Screen CurrentScreen { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WordtutorApplication"/> class
        /// </summary>
        /// <param name="initialScreen"></param>
        public WordTutorApplication(Screen initialScreen)
        {
            _priorScreens = ImmutableStack<Screen>.Empty;
            VocabularySet = VocabularySet.Empty;

            CurrentScreen = initialScreen ?? throw new ArgumentNullException(nameof(initialScreen));
        }

        /// <summary>
        /// Open a new screen, adding it to our stack
        /// </summary>
        /// <param name="screen">New screen to open.</param>
        public WordTutorApplication OpenScreen(Screen screen)
        {
            var screens = _priorScreens.Push(CurrentScreen);

            return new WordTutorApplication(
                this,
                currentScreen: screen ?? throw new ArgumentNullException(nameof(screen)),
            priorScreens: screens);
        }

        /// <summary>
        /// Closes the top screen on our stack, revealing the screen underneath
        /// </summary>
        /// <remarks>
        /// Won't close the top screen.
        /// </remarks>
        public WordTutorApplication CloseScreen()
        {
            if (_priorScreens.IsEmpty)
            {
                // Don't ever want to close the last screen
                return this;
            }

            return new WordTutorApplication(
                this,
                currentScreen: _priorScreens.Peek(),
                priorScreens: _priorScreens.Pop());
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

            return new WordTutorApplication(
                this, 
                currentScreen: screen);
        }

        private WordTutorApplication(
            WordTutorApplication original,
            VocabularySet vocabularySet = null,
            Screen currentScreen = null,
            ImmutableStack<Screen> priorScreens = null)
        {
            _priorScreens = priorScreens ?? original._priorScreens;
            CurrentScreen = currentScreen ?? original.CurrentScreen;
            VocabularySet = vocabularySet ?? original.VocabularySet;
        }
    }
}
