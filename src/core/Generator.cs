using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class Generator {
        public List<string> generatedNames { get; private set; }
        // list of saved presets <Settings>
        private Settings settings;
        

        public Generator() {
            generatedNames = new List<string>();
            settings = new Settings();
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
