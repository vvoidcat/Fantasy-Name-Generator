using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NAMEGEN.Control {
    public class PresetItem {
        public string title { get; private set; }
        public string filepath { get; private set; }
        public Brush color { get; private set; }
        public bool isPersistent { get; private set; }
        public bool isChecked { get; set; } = false;

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
