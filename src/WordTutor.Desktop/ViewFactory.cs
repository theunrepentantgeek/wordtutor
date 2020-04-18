using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using SimpleInjector;
using WordTutor.Core;

namespace WordTutor.Desktop
{
    /// <summary>
    /// Factory used to create a View based on the provided ViewModel
    /// </summary>
    /// <remarks>
    /// Relies on a naming convention where the type name of the View is based on replacing the 
    /// suffix "ViewModel" from the type name of the ViewModel type with either "View" or "Window".
    /// Also assumes the View will be located in the same assembly as the ViewModel.
    /// </remarks>
    public class ViewFactory
    {
        // Reference to the SimpleInjector container
        private readonly Container _container;

        // Cache of type mappings we already know
        private readonly Dictionary<Type, Type> _mapping
            = new Dictionary<Type, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewFactory"/> class
        /// </summary>
        /// <param name="container">Simple injector container.</param>
        public ViewFactory(Container container)
        {
            _container = container;
        }

        public ContentControl Create(ViewModelBase viewModel)
        {
            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var viewModelType = viewModel.GetType();
            if (!_mapping.TryGetValue(viewModelType, out var viewType))
            {
                viewType = FindViewType(viewModelType);
                _mapping[viewModelType] = viewType;
            }

            if (viewType == null)
            {
                throw new InvalidOperationException(
                    $"Failed to find ViewModel class for model type '{viewModelType.Name}'.");
            }

            var result = (ContentControl)_container.GetInstance(viewType);
            result.DataContext = viewModel;
            result.CommandBindings.AddRange(CreateCommandBindings(viewModel));

            viewModel.PropertyChanged += (s, a) => CommandManager.InvalidateRequerySuggested();

            return result;
        }

        public static Type FindViewType(Type viewModelType)
        {
            if (viewModelType is null)
            {
                throw new ArgumentNullException(nameof(viewModelType));
            }

            // Look for views in the Desktop assembly 
            // (might need changing if we introduce additional assemblies in future)
            var desktopAssembly = typeof(WordTutorWindow).Assembly;

            var viewModelTypeName = viewModelType.Name;
            var viewTypeName = viewModelTypeName.RemoveSuffix();
            var windowTypeName = viewTypeName.ReplaceSuffix("Window");
            var viewTypeNames = new HashSet<string>()
            {
                viewTypeName,
                windowTypeName
            };

            var viewType = (
                from type in desktopAssembly.GetExportedTypes()
                where type.IsClass && !type.IsAbstract
                where viewTypeNames.Contains(type.Name)
                select type
                ).SingleOrDefault();

            return viewType;
        }

        private static ICollection CreateCommandBindings(ViewModelBase model)
        {
            var commands =
                from property in model.GetType().GetProperties()
                where property.Name.EndsWith("Command", StringComparison.Ordinal)
                let command = property.GetValue(model) as RoutedCommandSinkBase
                where command != null
                select command.CreateBinding();
            var result = commands.ToList();
            return result;
        }
    }
}
