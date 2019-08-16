using System;
using System.Collections.Generic;
using System.Linq;
using SimpleInjector;
using WordTutor.Core;

namespace WordTutor.Desktop
{
    /// <summary>
    /// Factory used to create a ViewModel based on the provided model instance
    /// </summary>
    /// <remarks>
    /// Relies on a naming convention where the type name of the ViewModel is based on replacing 
    /// the suffix of the model type with the constant "ViewModel". 
    /// </remarks>
    public class ViewModelFactory
    {
        // Reference to the SimpleInjector container
        private readonly Container _container;

        // Cache of type mappings we already know
        private readonly Dictionary<Type, Type> _mapping
            = new Dictionary<Type, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelFactory"/> class
        /// </summary>
        /// <param name="container">Simple injector container.</param>
        public ViewModelFactory(Container container)
        {
            _container = container;
        }

        public ViewModelBase Create<T>(T state)
            where T : class
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            var modelType = state.GetType();
            if (!_mapping.TryGetValue(modelType, out var viewModelType))
            {
                viewModelType = FindViewModelType(modelType);
                _mapping[modelType] = viewModelType;
            }

            if (viewModelType == null)
            {
                throw new InvalidOperationException(
                    $"Failed to find ViewModel class for model type '{modelType.Name}'.");
            }

            return (ViewModelBase)_container.GetInstance(viewModelType);
        }

        public static Type FindViewModelType(Type modelType)
        {
            // Look for view models in the Desktop assembly 
            // (might need changing if we introduce additional assemblies in future)
            var desktopAssembly = typeof(ViewModelBase).Assembly;

            var modelTypeName = modelType.Name;
            var viewModelTypeNames = new HashSet<string>()
            {
                modelTypeName+"ViewModel",
                modelTypeName.ReplaceSuffix("ViewModel")
            };

            var viewModelType = (
                from type in desktopAssembly.GetExportedTypes()
                where type.IsClass && !type.IsAbstract
                where viewModelTypeNames.Contains(type.Name)
                select type
                ).SingleOrDefault();

            return viewModelType;
        }
    }
}
