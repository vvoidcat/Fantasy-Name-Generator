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
    public class Preset {
        public class NameRecord {
            [Index(0)] public string maleName { get; set; }
            [Index(1)] public string femaleName { get; set; }
        }
        
        public int minLength { get; private set; }
        public int maxLength { get; private set; }

        public int maxRowVows { get; set; }
        public int maxRowCons { get; set; }

        public double vowPercentageCorrection { get; set; }
        public double conPercentageCorrection { get; set; }

        public Gender gender { get; private set; }
        public Alphabet alphabet { get; private set; }
        public Matrix permutationMatrix { get; private set; }
        public Matrix probabilityMatrixStart { get; private set; }
        public Matrix probabilityMatrixEnd { get; private set; }


        private string filepath { get; set; }


        public Preset(string newPath, Language lang) {
            alphabet = new Alphabet(lang);
            filepath = newPath;

            gender = Gender.Neutral;

            // these should always be above 0
            minLength = 4;
            maxLength = 12;
            maxRowVows = 3;  // min 1, max 3
            maxRowCons = 3;

            vowPercentageCorrection = 0.0f;
            conPercentageCorrection = 0.0f;

            permutationMatrix = new Matrix(alphabet.lettersCount, alphabet.lettersCount);
            probabilityMatrixStart = new Matrix(1, alphabet.lettersCount);
            probabilityMatrixEnd = new Matrix(1, alphabet.lettersCount);

            ParseSourceTable();     // arg source table
        }

        public void UpdateSettings() {

        }

        private void ParseSourceTable() {
            // take path as an arg
            // check sha-sum of the csv, if it's changed - re-parse 
            // handle file not found situation -> delegate to ui?

            if (File.Exists(filepath)) {
                ReadSourceFile();
                permutationMatrix.NormalizeMatrix();
                probabilityMatrixStart.NormalizeMatrix();
                probabilityMatrixEnd.NormalizeMatrix();
            } else {
                permutationMatrix.SetMatrix(0.5f);
                probabilityMatrixStart.SetMatrix(0.5f);
                probabilityMatrixEnd.SetMatrix(0.5f);
            }
        }

        private void ReadSourceFile() {
            TextReader textReader = new StreamReader(filepath);

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
            //UpdateLengthParams(name);

            for (int i = 1; i < name.Length; i++) {
                int indexCurrent = i - 1;
                int indexNext = i;
                char currentLetterLower = char.ToLower(name[indexCurrent]);
                char nextLetterLower = char.ToLower(name[indexNext]);

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
                    probabilityMatrixStart.IncrementValueAtIndex(0, indexCurrent);
                } else if (i == name.Length - 1) {
                    probabilityMatrixEnd.IncrementValueAtIndex(0, indexNext);
                }

                permutationMatrix.IncrementValueAtIndex(indexCurrent, indexNext);
            }
        }
    }
}
