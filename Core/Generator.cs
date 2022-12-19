using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class Generator {
        Language language;
        private Alphabet alphabet;
        private List<string> generatedNames;
        private Gender chosenGender;

        public Generator() {
            language = Language.English;
            alphabet = new Alphabet(language);

            generatedNames = new List<string>();
            chosenGender = Gender.Neutral;
        }

        public static void updateSettings() {
            // 
        }

        public void GenerateName() {
            Name newName = new Name(alphabet, chosenGender, 3, 10);
            newName.Generate();
            generatedNames.Add(newName.namestring);       // this shouldnt be a stack

            //return newName.namestring;
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
