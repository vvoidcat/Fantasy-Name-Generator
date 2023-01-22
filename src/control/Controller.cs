using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAMEGEN.Core;

namespace NAMEGEN.Control {
    class Controller {
        //    string filepath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\elven_generic.csv";
        //    string filepath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\human_generic.csv";
        string filepath = @"D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\italian.csv";
        // string filepath = @"nosuchfile";

        Language lang = Language.English;
        public Gender gender = Gender.Neutral;

        public Preset currentPreset { get; private set; }
        private Generator gen;

        //private int minLen;
        //private int maxLen;
        //private int consMaxRow;
        //private int consMinRow;


        public Controller() {
            currentPreset = new Preset(filepath, lang);
            gen = new Generator(currentPreset);
        }
    }
}
