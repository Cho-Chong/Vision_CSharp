using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChongCore.Model
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Predicate<object> canExec;
        private Action<object> exec;

        public RelayCommand(Predicate<object> canExecute, Action<object> execute)
        {
            canExec = canExecute;
            exec = execute;
        }

        public bool CanExecute(object parameter)
        {
            return canExec(parameter);
        }

        public void Execute(object parameter)
        {
            exec(parameter);
        }
    }
}
