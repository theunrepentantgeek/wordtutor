using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace WordTutor.Desktop
{
    /// <summary>
    /// Base class for RoutedCommandSink implementations
    /// </summary>
    public abstract class RoutedCommandSinkBase : ICommand
    {
        // A reference to the command we implement
        private readonly RoutedCommand _command;

        // An optional reference to our command's host
        private readonly INotifyPropertyChanged? _commandHost;

        // Last value returned for CanExecute
        private bool _lastCanExecute;

        // Storage for our CanExecuteChanged event
        private EventHandler? _canExecuteChanged;

        /// <summary>
        /// Test to see if we can execute this command
        /// </summary>
        /// <param name="parameter">Data to be used by the command. Pass null if no data required.</param>
        /// <returns>True if this command is available, false otherwise.</returns>
        public bool CanExecute(object? parameter)
        {
            _lastCanExecute = CanExecuteCore(parameter);
            return _lastCanExecute;
        }

        /// <summary>
        /// Event triggered when our CanExecute value changes
        /// </summary>
        [SuppressMessage(
            "Potential Code Quality Issues",
            "RECS0020:Delegate subtraction has unpredictable result",
            Justification = "This use is safe.")]
        public event EventHandler? CanExecuteChanged
        {
            add => _canExecuteChanged += value;
            remove => _canExecuteChanged -= value;
        }

        /// <summary>
        /// Execute the actual command.
        /// </summary>
        /// <param name="parameter">Data to be used by the command. Pass null if no data required.</param>
        public abstract void Execute(object? parameter);

        /// <summary>
        /// Create a command binding for use by our ViewModels
        /// </summary>
        public CommandBinding CreateBinding()
        {
            return new CommandBinding(
                _command,
                (sender, e) => Execute(e.Parameter),
                (sender, e) => e.CanExecute = CanExecute(e.Parameter));
        }

        /// <summary>
        /// Initializes a new instance of the RoutedCommandSinkBase
        /// </summary>
        /// <param name="command">Command we provide.</param>
        protected RoutedCommandSinkBase(RoutedCommand command)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _commandHost = null;
        }

        /// <summary>
        /// Initializes a new instance of the RoutedCommandSinkBase
        /// </summary>
        /// <param name="command">Command we provide.</param>
        /// <param name="commandHost">Host instance providing the command.</param>
        protected RoutedCommandSinkBase(
            RoutedCommand command, 
            INotifyPropertyChanged commandHost)
            : this(command)
        {
            _commandHost = commandHost ?? throw new ArgumentNullException(nameof(commandHost));
            _commandHost.PropertyChanged += (sender, e) => OnCanExecuteChanged();
        }

        /// <summary>
        /// Trigger our <see cref="CanExecuteChanged"/> event
        /// </summary>
        protected void OnCanExecuteChanged()
        {
            var handler = _canExecuteChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Test to see if we can execute this command
        /// </summary>
        /// <param name="parameter">Data to be used by the command. Pass null if no data required.</param>
        /// <returns>True if this command is available, false otherwise.</returns>
        protected abstract bool CanExecuteCore(object? parameter);
    }
}
