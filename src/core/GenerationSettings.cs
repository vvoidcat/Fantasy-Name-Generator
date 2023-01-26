using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NAMEGEN.Core {
    public class GenerationSettings {
        public const int lengthLowerbound = 3;
        public const int lengthUpperbound = 12;
        public const int minRow = 1;
        public const int maxRow = 3;
        public const double correctionLowerbound = -0.5f;
        public const double correctionUpperbound = 0.5f;

        public Preset preset { get; private set; }
        public Gender gender { get; private set; } = Gender.Neutral;
        public int minLength { get; private set; } = lengthLowerbound;
        public int maxLength { get; private set; } = lengthUpperbound;

        public int maxRowCons { get; private set; } = 2;
        public int maxRowVows { get; private set; } = 2;

        public double conPercentageCorrection { get; private set; } = 0.0f;
        public double vowPercentageCorrection { get; private set; } = 0.0f;

        public bool allowConsRepeats { get; private set; } = true;
        public bool allowVowsRepeats { get; private set; } = true;

        public GenerationSettings(string sourcePath, string name, bool isEditable) {
            preset = new Preset(sourcePath, name, isEditable);
        }

        public bool SetPreset(Preset newPreset) {
            bool isValueSet = false;
            
            // if (preset != newPreset)
            preset = newPreset;
            isValueSet = true;

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

        public bool SetConCorrection(double newValue) {
            bool isValueSet = false;
            newValue = newValue / 100;

            if (newValue != conPercentageCorrection && newValue >= correctionLowerbound && newValue <= correctionUpperbound) {
                conPercentageCorrection = newValue;
                isValueSet = true;
            }
            return isValueSet;
        }

        public bool SetVowCorrection(double newValue) {
            bool isValueSet = false;
            newValue = newValue / 100;

            if (newValue != vowPercentageCorrection && newValue >= correctionLowerbound && newValue <= correctionUpperbound) {
                vowPercentageCorrection = newValue;
                isValueSet = true;
            }
            return isValueSet;
        }

        public bool SetAllowRepeatsCons(bool newIsAllowed) {
            bool isValueSet = false;

            if (newIsAllowed == allowConsRepeats) {
                allowConsRepeats = newIsAllowed;
                isValueSet = true;
            }

            return isValueSet;
        }

        public bool SetAllowRepeatsVows(bool newIsAllowed) {
            bool isValueSet = false;

            if (newIsAllowed == allowVowsRepeats) {
                allowVowsRepeats = newIsAllowed;
                isValueSet = true;
            }
            return isValueSet;
        }
    }
}
