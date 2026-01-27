using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BudgetHelperClassLibrary.Commands
{
    public class RelayCommand : ICommand
    {        
        private readonly Action<object?> _execute;
        private readonly Func<bool>? _canExecute;
        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object?> execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        //Backup ctor for problematic sickdays update
        //public RelayCommand(Func<Task> updateSickDays, Func<bool>? canUpdateSickdays = null)
        //{
        //    if (updateSickDays == null) throw new ArgumentNullException(nameof(updateSickDays));
        //    _execute = async _ => await updateSickDays().ConfigureAwait(false);
        //    _canExecute = canUpdateSickdays;
        //}

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}

