using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NAMEGEN.Control {
    public class RelayCommand<T> : ICommand {
        private Action<T> exec;
        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<T> action) {
            exec = action;
        }

        public bool CanExecute(object? parameter) {
            return true;
        }

        public void Execute(object? parameter) {
            exec.Invoke((T)parameter);
        }

        private void OnCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
