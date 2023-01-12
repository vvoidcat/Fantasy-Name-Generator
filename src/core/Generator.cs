using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class Generator {
        public List<string> generatedNames { get; private set; }
        // list of saved presets <Settings> ?
        public Preset currentPreset { get; private set; }


        public Generator(Preset newPreset) {
            generatedNames = new List<string>();
            currentPreset = newPreset;
        }

        public void UpdateSettings(Preset newPreset) {
            currentPreset = newPreset;
        }

        public void GenerateName() {
            Name newName = new Name(currentPreset);
            newName.Generate();
            generatedNames.Add(newName.namestring.ToString());

            if (generatedNames.Count > 100) {
                generatedNames.RemoveAt(0);
            }
        }

        public string GetLastName() {
            return generatedNames.Last();
        }

        public string GetNameAtIndex(int index) {
            return generatedNames.ElementAt(index);
        }

        public List<string> GetAllNames() {
            return generatedNames;
        }
    }
}
