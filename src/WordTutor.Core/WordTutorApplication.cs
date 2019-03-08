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
    }
}
