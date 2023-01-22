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
        class NameRecord {
            [Index(0)] public string maleName { get; set; } = "";
            [Index(1)] public string femaleName { get; set; } = "";
        }

        public string presetName { get; set; } = "Unsaved Preset";
        
        public int minLength { get; set; } = 3;     // write private setters
        public int maxLength { get; set; } = 12;

        public int maxRowVows { get; set; } = 2;
        public int maxRowCons { get; set; } = 2;

        public double vowPercentageCorrection { get; set; } = 0.0f;
        public double conPercentageCorrection { get; set; } = 0.0f;

        public bool allowConsRepeats { get; set; } = true;
        public bool allowVowsRepeats { get; set; } = true;

        public Alphabet alphabet { get; set; } = new Alphabet(Language.English);
        private string filepath { get; set; }

        public Matrix permutationMatrix { get; private set; }
        public Matrix probabilityMatrixStart { get; private set; }
        public Matrix probabilityMatrixEnd_Male { get; private set; }
        public Matrix probabilityMatrixEnd_Female { get; private set; }


        public Preset(string newPath, Language lang) {
            alphabet = new Alphabet(lang);
            filepath = newPath;

            permutationMatrix = new Matrix(alphabet.lettersCount, alphabet.lettersCount);
            probabilityMatrixStart = new Matrix(1, alphabet.lettersCount);
            probabilityMatrixEnd_Male = new Matrix(1, alphabet.lettersCount);
            probabilityMatrixEnd_Female = new Matrix(1, alphabet.lettersCount);

            ParseSourceTable();     // arg source table
        }

        private void ParseSourceTable() {
            // take path as an arg
            // check sha-sum of the csv, if it's changed - re-parse 
            // handle file not found situation -> delegate to ui?
            Matrix[] matrices = new Matrix[] { permutationMatrix, probabilityMatrixStart, probabilityMatrixEnd_Male, probabilityMatrixEnd_Female };

            if (File.Exists(filepath)) {
                ReadSourceFile();
            }

            foreach (Matrix m in matrices) {
                if (m.isZeroed) {
                    m.SetMatrix(0.5f);
                } else {
                    m.NormalizeMatrix();
                }
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
            if (!String.IsNullOrEmpty(record.maleName)) {
                ProcessNameStr(record.maleName, Gender.Male);
            }
            if (!String.IsNullOrEmpty(record.femaleName)) {
                ProcessNameStr(record.femaleName, Gender.Female);
            }
        }

        private void ProcessNameStr(string name, Gender gender) {
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
                    if (gender == Gender.Male) {
                        probabilityMatrixEnd_Male.IncrementValueAtIndex(0, indexNext);
                    } else if (gender == Gender.Female) {
                        probabilityMatrixEnd_Female.IncrementValueAtIndex(0, indexNext);
                    }
                }

                permutationMatrix.IncrementValueAtIndex(indexCurrent, indexNext);
            }
        }
    }
}
