using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class GenerationSettings {
        public Preset preset { get; set; }
        public Gender gender { get; set; } = Gender.Neutral;
        public int minLength { get; set; } = 3;     // write private setters
        public int maxLength { get; set; } = 12;

        public int maxRowVows { get; set; } = 2;
        public int maxRowCons { get; set; } = 2;

        public double vowPercentageCorrection { get; set; } = 0.0f;
        public double conPercentageCorrection { get; set; } = 0.0f;

        public bool allowConsRepeats { get; set; } = true;
        public bool allowVowsRepeats { get; set; } = true;

        public GenerationSettings(string sourcePath, string coverPath, string name, Language lang, bool isEditable) {
            preset = new Preset(sourcePath, coverPath, name, lang, isEditable);
        }
    }
}
