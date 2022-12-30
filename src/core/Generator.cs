using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class Generator {
        public Stack<string> generatedNames { get; private set; }
        // list of saved presets <Settings> ?
        public Settings currentPreset { get; private set; }


        public Generator(Settings newPreset) {
            generatedNames = new Stack<string>();
            currentPreset = newPreset;
        }

        public void UpdateSettings(Settings newPreset) {
            currentPreset = newPreset;
        }

        public void GenerateName() {
            Name newName = new Name(currentPreset);
            newName.Generate();
            generatedNames.Push(newName.namestring);
        }

        public string GetLastName() {
            return generatedNames.Last();
        }

        public string GetNameAtIndex(int index) {
            return generatedNames.ElementAt(index);
        }

        public Stack<string> GetAllNames() {
            return generatedNames;
        }
    }
}
