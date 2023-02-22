using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace NAMEGEN.Core {
    public class Name {
        public int length { get; private set; }
        public StringBuilder namestring { get; private set; }

        private const int maxRestrict = 2000;
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

            if (settings.selectedStartIndex >= 0 && letters.Count == 0) {
                index = settings.selectedStartIndex;
            } else if (settings.selectedEndIndex >= 0 && letters.Count == length - 1) {
                index = settings.selectedEndIndex;
            } else {
                index = ChooseIndex(rowConsonants, rowVowels, letters, gender);
            }
            return preset.alphabet.letters[index];
        }

        private int ChooseIndex(int rowConsonants, int rowVowels, List<Letter> letters, Gender gender) {
            int index = 0;
            int restrict = 0;
            bool isChosen = false;

            while (!isChosen) {
                index = ChooseRandomIndex();

                if (restrict < maxRestrict) {
                    isChosen = IsChoosableIndex(index, rowConsonants, rowVowels, letters, gender);
                } else {
                    if (preset.alphabet.letters[index].isConsonant == IsConsonantChosen(rowConsonants, rowVowels)
                        && IsAllowedRepeat(index, letters)
                        && IsAllowedPattern(index, letters)
                        && IsAllowedProbability(index, letters)) {
                        isChosen = true;
                        // throw an error message
                    }
                }
                restrict++;
            }
            return index;
        }

        private bool IsChoosableIndex(int index, int rowConsonants, int rowVowels, List<Letter> letters, Gender gender) {
            return (preset.alphabet.letters[index].isConsonant == IsConsonantChosen(rowConsonants, rowVowels)
                    && IsAllowedProbability(index, letters)
                    && IsAllowedPermutation(index, letters, gender)
                    && IsAllowedRepeat(index, letters)
                    && IsAllowedPattern(index, letters)) ? true : false;
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

        private bool IsAllowedPattern(int index, List<Letter> letters) {
            bool result = true;

            if (letters.Count >= 3) {
                string tempname = namestring.ToString().ToLower() + preset.alphabet.letters[index].lowercase;
                int minPatternSize = 2;
                int substrStartIndex = tempname.Length - minPatternSize;

                string substr = tempname.Substring(substrStartIndex);
                int matches = Regex.Matches(tempname, substr).Count;

                if (matches > settings.maxPatterns) {
                    result = false;
                }
            }
            return result;
        }

        private bool IsConsonantChosen(int currentConRow, int currentVowRow) {
            bool isConsonant = false;

            if (currentConRow == settings.maxRowCons
                || (namestring.Length == length - 1 && numVowels == 0)
                || (settings.selectedEndIndex >= 0
                    && preset.alphabet.letters[settings.selectedEndIndex].isConsonant
                    && currentConRow + 1 == settings.maxRowCons)) {
                isConsonant = false;
            } else if (currentVowRow == settings.maxRowVows
                       || (settings.selectedEndIndex >= 0
                           && preset.alphabet.letters[settings.selectedEndIndex].isVowel
                           && currentVowRow + 1 == settings.maxRowVows)) {
                isConsonant = true;
            } else {
                double percentage = 0.50f;

                if (currentVowRow > currentConRow) {
                    percentage = CalculatePercentage(currentVowRow, settings.maxRowVows);
                } else if (currentVowRow < currentConRow) {
                    percentage = CalculatePercentage(currentConRow, settings.maxRowCons);
                }
                isConsonant = CalculateResultFromPercentage(percentage);
            }
            return isConsonant;
        }

        private bool IsAllowedProbability(int index, List<Letter> letters) {
            return (letters.Count > 0) ? true : CalculateResultFromPercentage(preset.probabilityMatrixStart.GetValueAtIndex(0, index));
        }

        private bool IsAllowedPermutation(int index, List<Letter> letters, Gender gender) {
            double percentage = 100f;

            if (letters.Count == 1) {
                percentage = preset.permutationMatrixStart.GetValueAtIndex(letters.Last().index, index);
            } else if (letters.Count == length - 1) {
                percentage = GetPrefixPercentage(letters.Last().index, index, gender);
            } else if (letters.Count == length - 2 && settings.selectedEndIndex >= 0) {
                double percDefault = GetPrefixPercentage(letters.Last().index, index, gender);
                double percAdd = GetPrefixPercentage(index, settings.selectedEndIndex, gender);

                if (percDefault > 0f && percAdd > 0f) {
                    percentage = 100f;
                }
            } else if (letters.Count > 1) {
                percentage = preset.permutationMatrixGeneral.GetValueAtIndex(letters.Last().index, index);
            }
            return CalculateResultFromPercentage(percentage);
        }

        private double GetPrefixPercentage(int indexPrev, int indexNext, Gender gender) {
            double percentage = -100f;

            if (gender == Gender.Male) {
                percentage = preset.permutationMatrixEnd_Male.GetValueAtIndex(indexPrev, indexNext);
            } else if (gender == Gender.Female) {
                percentage = preset.permutationMatrixEnd_Female.GetValueAtIndex(indexPrev, indexNext);
            } else {
                percentage = GetMultipleMatrixPercentage(indexPrev, indexNext,
                    preset.permutationMatrixEnd_Male, preset.permutationMatrixEnd_Female);
            }
            return percentage;
        }

        private double GetMultipleMatrixPercentage(int indexPrev, int indexNext, Matrix m1, Matrix m2) {
            double perc1 = m1.GetValueAtIndex(indexPrev, indexNext);
            double perc2 = m2.GetValueAtIndex(indexPrev, indexNext);
            return (CalculateResultFromPercentage(perc1) || CalculateResultFromPercentage(perc2)) ? 100f : -100f;
        }

        private char ChooseLetterCase(int index, Letter newLetter) {
            return (index == 0) ? newLetter.uppercase : newLetter.lowercase;
        }

        private int ChooseRandomLength() {
            return random.Next(settings.minLength, settings.maxLength + 1);
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

        private double CalculatePercentage(int currentValue, int maxValue) {
            return (double)currentValue / maxValue;
        }

        private bool CalculateResultFromPercentage(double percentage) {
            return (percentage > 0f && (percentage >= 1f || random.NextDouble() <= percentage)) ? true : false;
        }
    }
}
