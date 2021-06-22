using System;
using System.ComponentModel;
using System.Windows.Input;

namespace WordTutor.Desktop
{
    /// <summary>
    /// A sink for routed commands that don't take a parameter value
    /// </summary>
    /// <remarks>
    /// Use instances of this to publish commands for triggering from elsewhere in the application.
    /// If your commands need a parameter, check out <see cref="RoutedCommandSink{T}"/>.
    /// Use the <see cref="RoutedCommandSinkBase.CreateBinding"/> method to create bindings for your WPF views.
    /// </remarks>
    public sealed class RoutedCommandSink : RoutedCommandSinkBase
    {
        // Reference to the action we perform
        private readonly Action _execute;

        // Reference to a predicate used to control access to the action we perform
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Initializes a new instance of the RoutedCommandSink class
        /// </summary>
        /// <param name="command">Command we provide.</param>
        /// <param name="execute">Action to trigger.</param>
        public RoutedCommandSink(RoutedCommand command, Action execute)
            : base(command)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = () => true;
        }

        /// <summary>
        /// Initializes a new instance of the RoutedCommandSink class
        /// </summary>
        /// <param name="command">Command we provide.</param>
        /// <param name="execute">Action to trigger.</param>
        /// <param name="canExecute">Predicate used to control access to the action.</param>
        public RoutedCommandSink(RoutedCommand command, Action execute, Func<bool> canExecute)
            : base(command)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        /// <summary>
        /// Initializes a new instance of the RoutedCommandSink class
        /// </summary>
        /// <param name="command">Command we provide.</param>
        /// <param name="execute">Action to trigger</param>
        /// <param name="canExecute">Predicate used to control access to the action.</param>
        /// <param name="host">Host that is providing the action.</param>
        public RoutedCommandSink(RoutedCommand command, Action execute, Func<bool> canExecute, INotifyPropertyChanged host)
            : base(command, host)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        /// <summary>
        /// Execute the actual command.
        /// </summary>
        /// <param name="parameter">Data to be used by the command. Pass null if no data required.</param>
        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _execute();
            }
        }

        /// <summary>
        /// Test to see if we can execute this command
        /// </summary>
        /// <param name="parameter">Data to be used by the command. Pass null if no data required.</param>
        /// <returns>True if this command is available, false otherwise.</returns>
        protected override bool CanExecuteCore(object? parameter)
        {
            return _canExecute == null || _canExecute();
        }
    }

    /// <summary>
    /// A sink for routed commands that take a parameter value
    /// </summary>
    /// <remarks>
    /// Use instances of this to publish commands for triggering from elsewhere in the application.
    /// If your commands need a parameter, check out <see cref="RoutedCommandSink"/>.
    /// Use the <see cref="RoutedCommandSinkBase.CreateBinding"/> method to create bindings for your WPF views.
    /// </remarks>

    public sealed class RoutedCommandSink<T> : RoutedCommandSinkBase
    {
        // Reference to the action we perform
        private readonly Action<T> _execute;

        // Reference to a predicate used to control access to the action we perform
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// Initializes a new instance of the RoutedCommandSink class
        /// </summary>
        /// <param name="command">Command we provide.</param>
        /// <param name="execute">Action to trigger</param>
        public RoutedCommandSink(
            RoutedCommand command,
            Action<T> execute)
            : base(command)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = _ => true;
        }

        /// <summary>
        /// Initializes a new instance of the RoutedCommandSink class
        /// </summary>
        /// <param name="command">Command we provide.</param>
        /// <param name="execute">Action to trigger</param>
        /// <param name="canExecute">Predicate used to control access to the action.</param>
        public RoutedCommandSink(
            RoutedCommand command,
            Action<T> execute,
            Func<T, bool> canExecute)
            : base(command)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        /// <summary>
        /// Initializes a new instance of the RoutedCommandSink class
        /// </summary>
        /// <param name="command">Command we provide.</param>
        /// <param name="execute">Action to trigger</param>
        /// <param name="canExecute">Predicate used to control access to the action.</param>
        /// <param name="host">Host that is providing the action.</param>
        public RoutedCommandSink(
            RoutedCommand command,
            Action<T> execute,
            Func<T, bool> canExecute,
            INotifyPropertyChanged host)
            : base(command, host)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        /// <summary>
        /// Execute the actual command.
        /// </summary>
        /// <param name="parameter">Data to be used by the command. Pass null if no data required.</param>
        public override void Execute(object? parameter)
        {
            if (!CanExecute(parameter))
            {
                return;
            }

            if (parameter is T actual)
            {
                _execute(actual);
            }
        }

        /// <summary>
        /// Test to see if we can execute this command
        /// </summary>
        /// <param name="parameter">Data to be used by the command. Pass null if no data required.</param>
        /// <returns>True if this command is available, false otherwise.</returns>
        protected override bool CanExecuteCore(object? parameter)
        {
            return parameter is T actual && _canExecute(actual);
        }
    }
}
