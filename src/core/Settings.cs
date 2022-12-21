using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace NAMEGEN.Core {
    public class Settings {
        public class NameRecord {
            [Index(0)] public string maleName { get; set; }
            [Index(1)] public string femaleName { get; set; }
            // familyName ?
        }

        public int minLength { get; private set; }
        public int maxLength { get; private set; }

        public int maxRowVows { get; private set; }
        public int maxRowCons { get; private set; }

        public double vowPercentageCorrection { get; private set; }
        public double conPercentageCorrection { get; private set; }

        public Gender gender { get; private set; }
        public Alphabet alphabet { get; private set; }
        public double[,] permutationMatrix { get; private set; }

        // probability matrix to count individual letter frequency
        // calculate vow and con percentage correction


        public Settings() {
            //textReader = new StreamReader("filename");

            alphabet = new Alphabet(Language.English);
            gender = Gender.Neutral;

            // these should always be above 0
            minLength = 3;
            maxLength = 10;
            maxRowVows = 2;
            maxRowCons = 3;

            vowPercentageCorrection = 0.0f;
            conPercentageCorrection = 0.0f;

            // allow consonant repeats
            // allow vowel repeats
            // allow special symbols

            ParseSourceTable();     // arg source table
        }

        public void UpdateSettings() {

        }

        private void ParseSourceTable() {
            TextReader textReader = new StreamReader("D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\human_generic.csv");

            // take path as an arg
            // check sha-sum of the csv, if it's changed - re-parse 
            // handle file not found situation

            InitPermutationMatrix(0.0f);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                HasHeaderRecord = false,
                MissingFieldFound = null
            };

            var csvReader = new CsvReader(textReader, config);

            while (csvReader.Read()) {
                var record = csvReader.GetRecord<NameRecord>();

                if (record is not null) {
                    UpdatePermutationMatrix((NameRecord)record);
                }
            }

            NormalizePermutationMatrix();


            //for (int i = 0; i < 26; i++) {
            //    for (int j = 0; j < 26; j++) {
            //        Console.Write(permutationMatrix[i, j] + " ");
            //    }
            //    Console.Write("\n");
            //}
        }

        private void InitPermutationMatrix(double value) {
            permutationMatrix = new double[alphabet.lettersCount, alphabet.lettersCount];

            for (int i = 0; i < alphabet.lettersCount; i++) {
                for (int j = 0; j < alphabet.lettersCount; j++) {
                    permutationMatrix[i, j] = value;
                }
            }
        }

        private void UpdatePermutationMatrix(NameRecord record) {
            if (!String.IsNullOrEmpty(record.maleName) && (gender == Gender.Male || gender == Gender.Neutral)) {
                ProcessNameStr(record.maleName);
            }
            if (!String.IsNullOrEmpty(record.femaleName) && (gender == Gender.Female || gender == Gender.Neutral)) {
                ProcessNameStr(record.femaleName);
            }
        }

        private void ProcessNameStr(string name) {
            for (int i = 1; i < name.Length; i++) {
                int indexCurrent = i - 1;
                int indexNext = i;
                string currentLetterLower = name[indexCurrent].ToString().ToLower();
                string nextLetterLower = name[indexNext].ToString().ToLower();

                for (int j = 0; j < alphabet.lettersCount; j++) {
                    Letter alphabetLetter = alphabet.letters[j];
                    if (currentLetterLower == alphabetLetter.lowercase) {
                        indexCurrent = alphabetLetter.index;
                    }
                    if (nextLetterLower == alphabetLetter.lowercase) {
                        indexNext = alphabetLetter.index;
                    }
                }

                SetMatrixValue(indexCurrent, indexNext);
            }
        }

        private void SetMatrixValue(int indexCurrent, int indexNext) {
            permutationMatrix[indexCurrent, indexNext] += 1.0f;
        }

        private void NormalizePermutationMatrix() {
            for (int i = 0; i < alphabet.lettersCount; i++) {
                double max = 1.0f;
                double sum = 0.0f;

                for (int j = 0; j < alphabet.lettersCount; j++) {
                    sum += permutationMatrix[i, j];
                }

                if (sum > 0.0f) {
                    double norm = max / sum;

                    for (int j = 0; j < alphabet.lettersCount; j++) {
                        permutationMatrix[i, j] = permutationMatrix[i, j] * norm;
                    }
                }
            }
        }
    }
}
