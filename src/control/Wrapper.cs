﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Control {
    public class Wrapper<T> : ObservableObject {
        private T _val;
        public T val {
            get { return _val; }
            set {
                _val = value;
                OnPropertyChanged(nameof(val));
            }
        }

        public Wrapper(T newValue) {
            _val = newValue;
        }
    }
}
