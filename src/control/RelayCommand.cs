using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NAMEGEN.Control {
    public class RelayCommand : CommandBase {
        private Action act;

        public RelayCommand(Action action) {
            act = action;
        }

        public override void Execute(object? parameter) {
            act();
        }
    }
}
