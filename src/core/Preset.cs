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

namespace NAMEGEN.Core {
    public class Preset {
        class NameRecord {
            [Index(0)] public string maleName { get; set; } = "";
            [Index(1)] public string femaleName { get; set; } = "";
        }
        
        public Alphabet alphabet { get; set; }

        public Matrix permutationMatrix { get; private set; }
        public Matrix probabilityMatrixStart { get; private set; }
        public Matrix probabilityMatrixEnd_Male { get; private set; }
        public Matrix probabilityMatrixEnd_Female { get; private set; }

        public string sourcepath { get; private set; } = "";
        public string sourceChecksum { get; private set; } = "";
        public string presetName { get; set; } = "Unsaved Preset";

        public Preset(string newSourcepath, string newName) {
            alphabet = new Alphabet();

            permutationMatrix = new Matrix(alphabet.letters.Count, alphabet.letters.Count);
            probabilityMatrixStart = new Matrix(1, alphabet.letters.Count);
            probabilityMatrixEnd_Male = new Matrix(1, alphabet.letters.Count);
            probabilityMatrixEnd_Female = new Matrix(1, alphabet.letters.Count);

            SetSourcepath(newSourcepath);

            presetName = newName;
        }


        public void SetSourcepath(string newSourcepath) {
            if (!File.Exists(newSourcepath) || Path.GetExtension(newSourcepath) != ".csv") {
                if (sourcepath == "") {
                    sourcepath = "generic";
                    FinalizeMatrices();
                    //throw new ArgumentException("aboba1");
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
            for (int i = 1; i < name.Length; i++) {
                int indexCurrent = i - 1;
                int indexNext = i;
                char currentLetterLower = char.ToLower(name[indexCurrent]);
                char nextLetterLower = char.ToLower(name[indexNext]);

                for (int j = 0; j < alphabet.letters.Count; j++) {
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

        private void FinalizeMatrices() {
            Matrix[] matrices = new Matrix[] { permutationMatrix, probabilityMatrixStart, probabilityMatrixEnd_Male, probabilityMatrixEnd_Female };

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
