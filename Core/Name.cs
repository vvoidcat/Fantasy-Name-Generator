using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class Name {
        // properties

        public int minLength { get; private set; }
        public int maxLength { get; private set; }

        public int numVowels { get; private set; }
        public int numConsonants { get; private set; }


        public int maxRowVows { get; private set; }
        public int numRowVowsCurrent { get; private set; }

        public int numRowConCurrent { get; private set; }
        public int maxRowCons { get; private set; }

        public double vowPercentageCorrection { get; private set; }
        public double conPercentageCorrection { get; private set; }

        public int length { get; private set; }
        public string namestring { get; private set; }

        public Gender gender { get; private set; }
        private Alphabet alphabet;
        private Random random;

        public Name(Alphabet chosenAlphabet, Gender nameGender, int nameMinLength, int nameMaxLength) {
            random = new Random();
            namestring = "";
            numVowels = 0;
            numConsonants = 0;

            // some of this should be stored in a separate settings object, initiated in a generator and passed here as an object
            // choosing settings should be enabled via an enum (presets)
            alphabet = chosenAlphabet;
            gender = nameGender;
            minLength = nameMinLength;
            maxLength = nameMaxLength;
            maxRowVows = 2;       // these should always be above 0
            maxRowCons = 3;
            vowPercentageCorrection = 0.0f;
            conPercentageCorrection = 0.0f;
            // allow consonant repeats
            // allow vowel repeats
            // allow special symbols
        }

        public void Generate() {
            LinkedList<Letter> letters = new LinkedList<Letter>();
            length = GetRandomLength();
            int rowConsonants = 0;
            int rowVowels = 0;

            for (int i = 0; i < length; i++) {
                Letter newLetter = ChooseLetter(IsConsonantChosen(rowConsonants, rowVowels));

                if (newLetter.isVowel) {
                    numVowels++;
                }

                if (newLetter.isConsonant) {
                    numConsonants++;
                }

                letters.Append(newLetter);

                namestring += ChooseLetterCase(i, newLetter);
                UpdateRowCounter(newLetter, ref rowConsonants, ref rowVowels);
            }
            letters.Clear();
        }



        private int GetRandomLength() {
            return random.Next(minLength, maxLength);
        }

        private Letter ChooseLetter(bool isConsonant) {
            int index = random.Next(0, alphabet.letters.Count - 1);
            Letter newLetter = alphabet.letters[index];

            if (newLetter.isConsonant != isConsonant) {
                newLetter = ChooseLetter(isConsonant);
            }

            return newLetter;
        }

        private string ChooseLetterCase(int index, Letter newLetter) {
            return (index == 0) ? newLetter.uppercase : newLetter.lowercase;
        }

        private bool IsConsonantChosen(int currentConRow, int currentVowRow) {
            bool isConsonant = false;

            if (currentConRow == maxRowCons || (namestring.Length == length - 1 && numVowels == 0)) {
                isConsonant = false;
            } else if (currentVowRow == maxRowVows) {
                isConsonant = true;
            } else {
                double percentage = 0.50f;

                if (currentVowRow > currentConRow) {
                    percentage = CalculatePercentage(currentVowRow, maxRowVows, vowPercentageCorrection);
                } else if (currentVowRow < currentConRow) {
                    percentage = CalculatePercentage(currentConRow, maxRowCons, conPercentageCorrection);
                }
                isConsonant = CalculateResultFromPercentage(percentage);
            }
            return isConsonant;
        }

        private void UpdateRowCounter(Letter chosenLetter, ref int currentConsonantRow, ref int currentVowelRow) {
            if (chosenLetter.isConsonant) {
                currentConsonantRow++;
                currentVowelRow = 0;
            } else {
                currentVowelRow++;
                currentConsonantRow = 0;
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
