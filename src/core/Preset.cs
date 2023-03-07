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
using System.Security.Cryptography;
using System.Reflection;
using System.Xml.Linq;

namespace NAMEGEN.Core {
    public class Preset {
        class NameRecord {
            [Index(0)] public string maleName { get; set; } = "";
            [Index(1)] public string femaleName { get; set; } = "";
        }
        
        public Alphabet alphabet { get; private set; }

        public Matrix permutationMatrixStart { get; private set; }
        public Matrix permutationMatrixGeneral { get; private set; }
        public Matrix permutationMatrixEnd_Male { get; private set; }
        public Matrix permutationMatrixEnd_Female { get; private set; }

        public Matrix probabilityMatrixStart { get; private set; }

        public string sourcepath { get; private set; } = "";
        public string sourceChecksum { get; private set; } = "";
        public string presetName { get; set; } = "New Preset";

        public Preset(Alphabet newAlphabet, string newName, string newSourcepath) {
            alphabet = newAlphabet;

            permutationMatrixStart = new Matrix(alphabet.letters.Count, alphabet.letters.Count);
            permutationMatrixGeneral = new Matrix(alphabet.letters.Count, alphabet.letters.Count);
            permutationMatrixEnd_Male = new Matrix(alphabet.letters.Count, alphabet.letters.Count);
            permutationMatrixEnd_Female = new Matrix(alphabet.letters.Count, alphabet.letters.Count);

            probabilityMatrixStart = new Matrix(1, alphabet.letters.Count);

            SetSourcepath(newSourcepath);

            presetName = newName;
        }

        private void SetSourcepath(string newSourcepath) {
            if (!File.Exists(newSourcepath) || Path.GetExtension(newSourcepath) != ".csv") {
                if (sourcepath == "") {
                    sourcepath = "generic";
                    FinalizeMatrices();
                    //throw new ArgumentException("aboba1");    // inititlize default state without a model
                }
                //else {
                //    throw new ArgumentException("aboba2");    // = last path was ok, the new one is unacceptable, leave the last one be
                //}
            } else {
                string newChecksum = sourceChecksum;

                if (sourcepath == newSourcepath) {
                    newChecksum = GetChecksum(newSourcepath);
                }

                if (sourceChecksum != newChecksum) {
                    ParseSourceTable();
                    sourcepath = newSourcepath;
                    sourceChecksum = newChecksum;
                }

                sourcepath = newSourcepath;
                sourceChecksum = newChecksum;
                ParseSourceTable();
                FinalizeMatrices();
            }
        }

        private string GetChecksum(string path) {
            SHA256 SHA256 = SHA256Managed.Create();

            FileStream fileStream = File.OpenRead(path);
            string newChecksum = BitConverter.ToString(SHA256.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
            return newChecksum;
        }

        private void ParseSourceTable() {
            if (File.Exists(sourcepath)) {
                ReadSourceFile();
            }
        }

        private void ReadSourceFile() {
            TextReader textReader = new StreamReader(sourcepath);

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
            for (int i = 0; i < name.Length - 1; i++) {
                int indexCurrent = i;
                int indexNext = i + 1;
                char currentLetterLower = char.ToLower(name[indexCurrent]);
                char nextLetterLower = char.ToLower(name[indexNext]);

                UpdateWorkingIndexes(currentLetterLower, nextLetterLower, ref indexCurrent, ref indexNext);
                UpdateMatrices(name, gender, i, indexCurrent, indexNext);
            }
        }

        private void UpdateWorkingIndexes(char current, char next, ref int indexCurrent, ref int indexNext) {
            for (int j = 0; j < alphabet.letters.Count; j++) {
                Letter alphabetLetter = alphabet.letters[j];
                if (current == alphabetLetter.lowercase) {
                    indexCurrent = alphabetLetter.index;
                }
                if (next == alphabetLetter.lowercase) {
                    indexNext = alphabetLetter.index;
                }
            }
        }

        private void UpdateMatrices(string name, Gender gender, int i, int indexCurrent, int indexNext) {
            permutationMatrixGeneral.IncrementValueAtIndex(indexCurrent, indexNext);

            if (i == 0) {
                permutationMatrixStart.IncrementValueAtIndex(indexCurrent, indexNext);
                probabilityMatrixStart.IncrementValueAtIndex(0, indexCurrent);
            } else if (i == name.Length - 2) {
                if (gender == Gender.Male) {
                    permutationMatrixEnd_Male.IncrementValueAtIndex(indexCurrent, indexNext);
                } else if (gender == Gender.Female) {
                    permutationMatrixEnd_Female.IncrementValueAtIndex(indexCurrent, indexNext);
                }
            }
        }

        private void FinalizeMatrices() {
            Matrix[] matrices = new Matrix[] {
                permutationMatrixStart,
                permutationMatrixGeneral,
                permutationMatrixEnd_Male,
                permutationMatrixEnd_Female,
                probabilityMatrixStart };

            foreach (Matrix m in matrices) {
                if (m.isZeroed) {
                    m.SetMatrix(0.5f);
                } else {
                    m.Normalize();
                }
            }
        }
    }
}
