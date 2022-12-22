using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class Generator {
        public List<string> generatedNames { get; private set; }
        // list of saved presets <Settings>
        public Settings settings { get; private set; }


        public Generator(string filepath) {
            generatedNames = new List<string>();
            settings = new Settings(filepath);
        }

        public static void updateSettings() {
            // 
        }

        public void GenerateName() {
            Name newName = new Name(settings);
            newName.Generate();
            generatedNames.Add(newName.namestring);
        }

        public string GetLastName() {
            return generatedNames.Last();
        }

        public List<string> GetAllNames() {
            return generatedNames;
        }

        //private static 
    }
}
