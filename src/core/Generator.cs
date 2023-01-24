using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class Generator {
        public List<string> generatedNames { get; private set; }
        // list of saved presets <Settings> ?
        //public Preset currentPreset { get; private set; }
        GenerationSettings settings;


        public Generator(GenerationSettings currentSettings) {
            generatedNames = new List<string>();
            settings = currentSettings;
        }

        public void UpdateSettings(GenerationSettings newSettings) {
            settings = newSettings;
        }

        public void GenerateName() {
            Name newName = new Name(settings);
            newName.Generate(settings.gender);
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
