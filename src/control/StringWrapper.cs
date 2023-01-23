using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Control {
    public class StringWrapper : ObservableObject {
        private string _str;
        public string str {
            get { return _str; }
            set {
                _str = value;
                OnPropertyChanged(nameof(str));
            }
        }

        public StringWrapper(string newValue) {
            _str = newValue;
        }
    }
}
