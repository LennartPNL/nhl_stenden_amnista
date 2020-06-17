using System;
using System.Windows.Input;

namespace Amnista.Generic
{
    /// <summary>
    /// This commander takes any action and executes when allowed
    /// </summary>
    class Commander : ICommand
    {
        private readonly Action _action;
        public event EventHandler CanExecuteChanged;

        public Commander(Action action) => _action = action;

        /// <summary>
        /// Determines if the action is allowed. In our case we always return true because we have no different access levels
        /// </summary>
        /// <param name="parameter">Object that can be used to check if the action is allowed or not</param>
        /// <returns>True if action is allowed</returns>
        public bool CanExecute(object parameter) => true;

        /// <summary>
        /// Executes the given action
        /// </summary>
        /// <param name="parameter">Parameter for the action</param>
        public void Execute(object parameter) => _action();
    }
}