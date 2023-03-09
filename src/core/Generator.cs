using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class Generator {
        public const int lengthLowerbound = 4;
        public const int lengthUpperbound = 12;
        public const int minRow = 1;
        public const int maxRow = 3;

        public Alphabet alphabet { get; private set; }

        public Preset preset { get; private set; }
        public Gender gender { get; private set; } = Gender.Neutral;

        public int minLength { get; private set; } = 5;
        public int maxLength { get; private set; } = 10;

        public int maxRowCons { get; private set; } = 2;
        public int maxRowVows { get; private set; } = 2;

        public bool allowConsRepeats { get; private set; } = true;
        public bool allowVowsRepeats { get; private set; } = true;

        public int maxPatterns { get; private set; } = 2;

        public int selectedStartIndex { get; private set; } = -1;
        public int selectedEndIndex { get; private set; } = -1;


        public Generator(string sourcePath) {
            alphabet = new Alphabet();
            preset = new Preset(alphabet, sourcePath);
        }

        public string GenerateName() {
            Name newName = new Name(this);
            newName.Generate(gender);
            return newName.namestring.ToString();
        }

        public bool SetPreset(string newPath) {
            bool isValueSet = false;

            if (newPath != preset.sourcepath) {
                preset = new Preset(alphabet, newPath);
                isValueSet = true;
            }
            return isValueSet;
        }

        public bool SetGender(Gender newGender) {
            bool isValueSet = false;

            if (gender != newGender) {
                gender = newGender;
                isValueSet = true;
            }
            return isValueSet;
        }

        public bool SetGender(int newIndex) {
            bool isValueSet = false;

            if (Enum.IsDefined(typeof(Gender), newIndex)) {
                isValueSet = SetGender((Gender)newIndex);
            }
            return isValueSet;
        }

        public bool SetMinLength(int newValue) {
            bool isValueSet = false;

            if (newValue != minLength && newValue >= lengthLowerbound && newValue < maxLength) {
                minLength = newValue;
                isValueSet = true;
            }
            return isValueSet;
        }

        public bool SetMaxLength(int newValue) {
            bool isValueSet = false;

            if (newValue != maxLength && newValue <= lengthUpperbound && newValue > minLength) {
                maxLength = newValue;
                isValueSet = true;
            }
            return isValueSet;
        }

        public bool SetMaxRowCons(int newValue) {
            bool isValueSet = false;

            if (newValue != maxRowCons && newValue >= minRow && newValue <= maxRow) {
                maxRowCons = newValue;
                isValueSet = true;
            }
            return isValueSet;
        }

        public bool SetMaxRowVows(int newValue) {
            bool isValueSet = false;

            if (newValue != maxRowVows && newValue >= minRow && newValue <= maxRow) {
                maxRowVows = newValue;
                isValueSet = true;
            }
            return isValueSet;
        }

        public bool SetAllowRepeatsCons(bool newIsAllowed) {
            bool isValueSet = false;

            if (newIsAllowed != allowConsRepeats) {
                allowConsRepeats = newIsAllowed;
                isValueSet = true;
            }

            return isValueSet;
        }

        public bool SetAllowRepeatsVows(bool newIsAllowed) {
            bool isValueSet = false;

            if (newIsAllowed != allowVowsRepeats) {
                allowVowsRepeats = newIsAllowed;
                isValueSet = true;
            }
            return isValueSet;
        }

        public bool SetMaxPatterns(int newValue) {
            bool isValueSet = false;

            if (newValue != maxPatterns && newValue >= minRow && newValue <= maxRow) {
                maxPatterns = newValue;
                isValueSet = true;
            }
            return isValueSet;
        }

        public bool SetSelectedIndexStart(int newValue) {
            bool isValueSet = false;

            if ((newValue - 1) != selectedStartIndex) {
                selectedStartIndex = newValue - 1;
                isValueSet = true;
            }
            return isValueSet;
        }

        public bool SetSelectedIndexEnd(int newValue) {
            bool isValueSet = false;

            if ((newValue - 1) != selectedEndIndex) {
                selectedEndIndex = newValue - 1;
                isValueSet = true;
            }
            return isValueSet;
        }
    }
}
