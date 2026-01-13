using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BudgetHelperMS.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _executeCommand;
        private readonly Func<bool>? _canExecuteCheck;
        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action executeCommand, Func<bool>? canExecuteCheck=null)
        {
            _executeCommand = executeCommand;
            _canExecuteCheck = canExecuteCheck;
        }

        public bool CanExecute(object? parameter) => _canExecuteCheck?.Invoke() ?? true;

        public void Execute(object? parameter)=>_executeCommand();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
