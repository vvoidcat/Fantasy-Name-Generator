using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAMEGEN.Core;

namespace NAMEGEN.Control {
    internal class GenerateCommand : CommandBase {
        private Preset currentPreset;
        private Generator gen;

        public GenerateCommand(Preset preset, Gender gender) {
            currentPreset = preset;
            gen = new Generator(preset);
        }

        public override void Execute(object? parameter) {
            //
        }
    }
}
