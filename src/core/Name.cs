using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class Name {
        public int numVowels { get; private set; }
        public int numConsonants { get; private set; }
        public int length { get; private set; }
        public string namestring { get; private set; }

        private Random random;
        private Settings settings;

        public Name(Settings chosenSettings) {
            random = new Random();
            settings = chosenSettings;
            namestring = "";
            numVowels = 0;
            numConsonants = 0;
        }

        public void Generate() {
            List<Letter> letters = new List<Letter>();
            length = ChooseRandomLength();
            int rowConsonants = 0;
            int rowVowels = 0;

            for (int i = 0; i < length; i++) {
                Letter newLetter = ChooseLetter(rowConsonants, rowVowels, letters);
                letters.Add(newLetter);
                namestring += ChooseLetterCase(i, newLetter);
                UpdateCounters(newLetter, ref rowConsonants, ref rowVowels);
            }
            letters.Clear();
        }

        private Letter ChooseLetter(int rowConsonants, int rowVowels, List<Letter> letters) {
            int index = 0;
            bool isChosen = false;

            while (!isChosen) {
                index = ChooseRandomIndex();
                isChosen = IsChoosableIndex(index, rowConsonants, rowVowels, letters);
            }
            return settings.alphabet.letters[index];
        }

        private bool IsChoosableIndex(int index, int rowConsonants, int rowVowels, List<Letter> letters) {
            return (settings.alphabet.letters[index].isConsonant == IsConsonantChosen(rowConsonants, rowVowels)
                    && IsAllowedPermutation(index, letters)
                    && IsAllowedProbability(index, letters)
                    && IsAllowedRepeat(index, letters)) ? true : false;
        }

        private bool IsAllowedProbability(int index, List<Letter> letters) {
            return (letters.Count <= 0) ? CalculateResultFromPercentage(settings.probabilityMatrixStart[0, index]) :
                        (letters.Count == length - 1) ? CalculateResultFromPercentage(settings.probabilityMatrixEnd[0, index]) : true;
        }

        private bool IsAllowedPermutation(int index, List<Letter> letters) {
            return (letters.Count > 0) ? CalculateResultFromPercentage(settings.permutationMatrix[letters.Last().index, index]) : true;
        }

        private bool IsAllowedRepeat(int index, List<Letter> letters) {
            bool result = true;

            if (letters.Count >= 2) {
                Letter chosenLetter = settings.alphabet.letters[index];
                Letter lastLetter = letters[letters.Count - 1];
                Letter prevLetter = letters[letters.Count - 2];

                if (lastLetter == prevLetter && lastLetter == chosenLetter) {
                    result = false;
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

        private string ChooseLetterCase(int index, Letter newLetter) {
            return (index == 0) ? newLetter.uppercase : newLetter.lowercase;
        }

        private int ChooseRandomLength() {
            return random.Next(settings.minLength, settings.maxLength);
        }

        private int ChooseRandomIndex() {
            return random.Next(0, settings.alphabet.letters.Count - 1);
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
