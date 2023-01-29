using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class Name {
        public int length { get; private set; }
        public StringBuilder namestring { get; private set; }
        private Random random;
        private Preset preset;
        private Generator settings;
        private int numVowels;
        private int numConsonants;

        public Name(Generator gen) {
            random = new Random();
            namestring = new StringBuilder();
            settings = gen;
            preset = gen.preset;
            numVowels = 0;
            numConsonants = 0;
        }

        public void Generate(Gender gender) {
            List<Letter> letters = new List<Letter>();
            length = ChooseRandomLength();
            int rowConsonants = 0;
            int rowVowels = 0;

            for (int i = 0; i < length; i++) {
                Letter newLetter = ChooseLetter(rowConsonants, rowVowels, letters, gender);
                letters.Add(newLetter);
                namestring.Append(ChooseLetterCase(i, newLetter));
                UpdateCounters(newLetter, ref rowConsonants, ref rowVowels);
            }
            letters.Clear();
        }

        private Letter ChooseLetter(int rowConsonants, int rowVowels, List<Letter> letters, Gender gender) {
            int index = 0;
            bool isChosen = false;

            while (!isChosen) {
                index = ChooseRandomIndex();
                isChosen = IsChoosableIndex(index, rowConsonants, rowVowels, letters, gender);
            }
            return preset.alphabet.letters[index];
        }

        private bool IsChoosableIndex(int index, int rowConsonants, int rowVowels, List<Letter> letters, Gender gender) {
            return (preset.alphabet.letters[index].isConsonant == IsConsonantChosen(rowConsonants, rowVowels)
                    && IsAllowedProbability(index, letters, gender)
                    && IsAllowedRepeat(index, letters)) ? true : false;
        }

        private bool IsAllowedProbability(int index, List<Letter> letters, Gender gender) {
            double percentage;

            if (letters.Count <= 0) {
                percentage = preset.probabilityMatrixStart.GetValueAtIndex(0, index);
            } else if (letters.Count == length - 1) {
                if (gender == Gender.Male) {
                    percentage = preset.probabilityMatrixEnd_Male.GetValueAtIndex(0, index);
                } else if (gender == Gender.Female) {
                    percentage = preset.probabilityMatrixEnd_Female.GetValueAtIndex(0, index);
                } else {
                    percentage = preset.permutationMatrix.GetValueAtIndex(letters.Last().index, index);
                }
            } else {
                percentage = preset.permutationMatrix.GetValueAtIndex(letters.Last().index, index);
            }

            bool result = CalculateResultFromPercentage(percentage);
            return result;
        }

        private bool IsAllowedRepeat(int index, List<Letter> letters) {
            bool result = true;
            int count = letters.Count;

            if (count >= 1) {
                Letter chosenLetter = preset.alphabet.letters[index];
                Letter lastLetter = letters[letters.Count - 1];

                if (chosenLetter == lastLetter
                    && ((chosenLetter.isConsonant && !settings.allowConsRepeats)
                    || (chosenLetter.isVowel && !settings.allowVowsRepeats))) {
                    result = false;
                }

                if (count >= 2 && result) {
                    Letter prevLetter = letters[letters.Count - 2];

                    if (chosenLetter == lastLetter && chosenLetter == prevLetter) {
                        result = false;
                    }
                }
            }
            return result;
        }

        private bool IsConsonantChosen(int currentConRow, int currentVowRow) {
            bool isConsonant = false;

            if (currentConRow == settings.maxRowCons || (namestring.Length == length - 1 && numVowels == 0)) {
                isConsonant = false;
            } else if (currentVowRow == settings.maxRowVows) {
                isConsonant = true;
            } else {
                double percentage = 0.50f;

                if (currentVowRow > currentConRow) {
                    percentage = CalculatePercentage(currentVowRow, settings.maxRowVows, settings.vowPercentageCorrection);
                } else if (currentVowRow < currentConRow) {
                    percentage = CalculatePercentage(currentConRow, settings.maxRowCons, settings.conPercentageCorrection);
                }
                isConsonant = CalculateResultFromPercentage(percentage);
            }
            return isConsonant;
        }

        private char ChooseLetterCase(int index, Letter newLetter) {
            return (index == 0) ? newLetter.uppercase : newLetter.lowercase;
        }

        private int ChooseRandomLength() {
            return random.Next(settings.minLength, settings.maxLength);
        }

        private int ChooseRandomIndex() {
            return random.Next(0, preset.alphabet.letters.Count - 1);
        }

        private void UpdateCounters(Letter chosenLetter, ref int currentConsonantRow, ref int currentVowelRow) {
            if (chosenLetter.isConsonant) {
                currentConsonantRow++;
                currentVowelRow = 0;
                numConsonants++;
            } else {
                currentVowelRow++;
                currentConsonantRow = 0;
                numVowels++;
            }
        }

        private double CalculatePercentage(int currentValue, int maxValue, double correction) {
            return ((double)currentValue / maxValue) + correction;
        }

        private bool CalculateResultFromPercentage(double percentage) {
            double num = random.NextDouble();
            return (num <= percentage) ? true : false;
        }
    }
}
