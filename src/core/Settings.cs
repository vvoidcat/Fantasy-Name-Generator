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
using System.Data;

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
        public double[,] probabilityMatrix { get; private set; }

        // probability matrix to count individual letter frequency (now influences only the first letter choice)
        // calculate vow and con percentage correction

        private string filepath { get; set; }


        public Settings(string newPath) {
            alphabet = new Alphabet(Language.English);
            gender = Gender.Neutral;

            filepath = newPath;

            // these should always be above 0
            minLength = 4;
            maxLength = 10;
            maxRowVows = 2;  // min 1, max 3
            maxRowCons = 3;

            vowPercentageCorrection = 0.0f;
            conPercentageCorrection = 0.0f;

            // allow special symbols

            // first letter(s) separate generation
            // last letter(s) separate generation based on gender (consonant choice percentage correction)
            // max row percentage correction
            // name length percentage correction variable

            ParseSourceTable();     // arg source table
        }

        public void UpdateSettings() {

        }

        private void ParseSourceTable() {
            TextReader textReader = new StreamReader(filepath);

            // take path as an arg
            // check sha-sum of the csv, if it's changed - re-parse 
            // handle file not found situation -> delegate to ui?

            permutationMatrix = InitMatrix(alphabet.lettersCount, alphabet.lettersCount, 0.0f);
            probabilityMatrix = InitMatrix(1, alphabet.lettersCount, 0.0f);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                HasHeaderRecord = false,
                MissingFieldFound = null
            };

            var csvReader = new CsvReader(textReader, config);

            while (csvReader.Read()) {
                var record = csvReader.GetRecord<NameRecord>();

                if (record is not null) {
                    UpdateMatrices((NameRecord)record);
                }
            }

            NormalizeMatrix(permutationMatrix);
            NormalizeMatrix(probabilityMatrix);
        }

        private double[,] InitMatrix(int y, int x, double value) {
            double[,] matrix = new double[y, x];

            for (int i = 0; i < y; i++) {
                for (int j = 0; j < x; j++) {
                    matrix[i, j] = value;
                }
            }
            return matrix;
        }

        private void UpdateMatrices(NameRecord record) {
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

                    //SetProbabilityMatrixValue()
                }

                if (i == 1) {
                    IncrementMatrixValue(probabilityMatrix, 0, indexCurrent);
                }

                IncrementMatrixValue(permutationMatrix, indexCurrent, indexNext);
            }
        }

        private void IncrementMatrixValue(double[,] matrix, int indexY, int indexX) {
            matrix[indexY, indexX] += 1.0f;
        }

        private void NormalizeMatrix(double[,] matrix) {
            int y = matrix.Length / alphabet.lettersCount;

            for (int i = 0; i < y; i++) {
                double max = 1.0f;
                double sum = 0.0f;

                for (int j = 0; j < alphabet.lettersCount; j++) {
                    sum += matrix[i, j];
                }

                if (sum > 0.0f) {
                    double norm = max / sum;

                    for (int j = 0; j < alphabet.lettersCount; j++) {
                        matrix[i, j] = matrix[i, j] * norm;
                    }
                }
            }
        }
    }
}
