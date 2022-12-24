﻿using System;
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

        public int minLength { get; private set; }  // + probability
        public int maxLength { get; private set; }  // + probability

        public int maxRowVows { get; set; }
        public int maxRowCons { get; set; }

        public double vowPercentageCorrection { get; set; }
        public double conPercentageCorrection { get; set; }

        public Gender gender { get; private set; }
        public Alphabet alphabet { get; private set; }
        public double[,] permutationMatrix { get; private set; }
        public double[,] probabilityMatrixStart { get; private set; }
        public double[,] probabilityMatrixEnd { get; private set; }


        private string filepath { get; set; }


        public Settings(string newPath) {
            alphabet = new Alphabet(Language.English);
            gender = Gender.Neutral;

            filepath = newPath;

            // these should always be above 0
            minLength = 4;
            maxLength = 12;
            maxRowVows = 3;  // min 1, max 3
            maxRowCons = 3;

            vowPercentageCorrection = 0.0f;
            conPercentageCorrection = 0.0f;

            // same letter repetition percentage chance (con and vow separate)

            // allow special symbols

            // forbid names > 3 that consist of the same letters (repeated con, repeated vow = Annanana)
            // forbid > 3 repetitions of the same permutation (nanana, lalala)
            // max row percentage correction
            // calculate vow and con percentage correction
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
            probabilityMatrixStart = InitMatrix(1, alphabet.lettersCount, 0.0f);
            probabilityMatrixEnd = InitMatrix(1, alphabet.lettersCount, 0.0f);


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
            NormalizeMatrix(probabilityMatrixStart);
            NormalizeMatrix(probabilityMatrixEnd);
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

        private void UpdateLengthParams(string name) {
            if (name.Length > 0) {
                if (name.Length < minLength) {
                    minLength = name.Length;
                }
                if (name.Length > maxLength) {
                    maxLength = name.Length;
                }
            }
        }

        private void ProcessNameStr(string name) {
            //UpdateLengthParams(name);

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

                if (i == 1) {
                    IncrementMatrixValue(probabilityMatrixStart, 0, indexCurrent);
                } else if (i == name.Length - 1) {
                    IncrementMatrixValue(probabilityMatrixEnd, 0, indexNext);
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
