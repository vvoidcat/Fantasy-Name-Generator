using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAMEGEN.Core;

namespace NAMEGEN.Control {
    internal class GenerateCommand : CommandBase {
        private Preset currentPreset;
        private Gender chosenGender;
        private Generator gen;

        public GenerateCommand(Preset preset, Gender gender) {
            currentPreset = preset;
            chosenGender = gender;
            gen = new Generator(currentPreset);
        }

        public override void Execute(object? parameter) {
            gen.GenerateName(chosenGender);

            List<string> allNames = gen.GetAllNames();

            //    if (allNames is not null) {
            //        for (int i = 0; i < nameBlocks.Count && i < allNames.Count; i++) {
            //            nameBlocks[i].Text = gen.GetNameAtIndex(allNames.Count - (1 + i));
            //        }
            //    }
        }
    }
}
