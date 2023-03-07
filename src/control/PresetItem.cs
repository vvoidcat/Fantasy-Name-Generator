using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NAMEGEN.Control {
    public class PresetItem : ObservableObject {
        private string _title;
        public string title { 
            get { return _title; }
            set {
                if (_title != value) {
                    _title = value;
                    OnPropertyChanged(nameof(title));
                }
            }
        }

        private string _filepath;
        public string filepath {
            get { return _filepath; }
            set {
                if (filepath != value) {
                    _filepath = value;
                    OnPropertyChanged(nameof(filepath));
                }
            }
        }

        private bool _isSelected = false;
        public bool isSelected {
            get { return _isSelected; }
            set {
                if (_isSelected != value) {
                    _isSelected = value;
                    OnPropertyChanged(nameof(isSelected));
                }
            }
        }

        public Brush color { get; private set; }
        public bool isPersistent { get; private set; }

        public PresetItem(string newTitle) : this(newTitle, "", false, Brushes.Transparent) { }

        public PresetItem(string newTitle, string newPath, bool newIsPersistent, Brush selectedBrush) {
            title = newTitle;
            filepath = newPath;
            color = selectedBrush;
            isPersistent = newIsPersistent;
        }

        public static bool operator ==(PresetItem p1, PresetItem p2) {
            return (p1 is not null && p2 is not null && p1.title == p2.title) ? true : false;
        }

        public static bool operator !=(PresetItem p1, PresetItem p2) {
            return (p1 == p2) ? false : true;
        }

        public override bool Equals(object? obj) {
            return (obj is not null && obj is PresetItem && (PresetItem)obj == this) ? true : false;
        }
    }
}
