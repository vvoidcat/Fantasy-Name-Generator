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
        }

        public int minLength { get; private set; }
        public int maxLength { get; private set; }

        public int maxRowVows { get; private set; }
        public int maxRowCons { get; private set; }

        public double vowPercentageCorrection { get; private set; }
        public double conPercentageCorrection { get; private set; }

        public Gender gender { get; private set; }
        public Alphabet alphabet { get; private set; }
        public double[,] probabilityMatrix { get; private set; }

        //private TextReader textReader;


        public Settings() {
            //textReader = new StreamReader("filename");

            alphabet = new Alphabet(Language.English);
            gender = Gender.Neutral;

            // these should always be above 0
            minLength = 3;
            maxLength = 11;
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

            InitProbabilityMatrix();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                HasHeaderRecord = false,
            };

            var csvReader = new CsvReader(textReader, config);

            while (csvReader.Read()) {
                var record = csvReader.GetRecord<NameRecord>();

                if (record is not null) {
                    Console.WriteLine("male: {0} | female: {1}", record.maleName, record.femaleName);
                    UpdateProbabilityMatrix(record);
                }
            }
        }

        private void InitProbabilityMatrix() {
            probabilityMatrix = new double[alphabet.lettersCount, alphabet.lettersCount];

            for (int i = 0; i < alphabet.lettersCount; i++) {
                for (int j = 0; j < alphabet.lettersCount; j++) {
                    probabilityMatrix[i, j] = 0.5f;
                }
            }
        }

        private void UpdateProbabilityMatrix(NameRecord record) {
            if (gender == Gender.Male || gender == Gender.Neutral) {
                // update with male name
            } else if (gender == Gender.Female || gender == Gender.Neutral) {
                // update with female name
            }

            //for (int i = 0; i < alphabet.lettersCount; i++) {
            //    probabilityMatrix[0, i] = 0.001f;
            //    probabilityMatrix[4, i] = 0.001f;

            //    probabilityMatrix[22, i] = 0.001f;
            //    probabilityMatrix[i, 22] = 0.001f;

            //    probabilityMatrix[23, i] = 0.001f;
            //    probabilityMatrix[i, 23] = 0.001f;

            //    probabilityMatrix[25, i] = 0.001f;
            //    probabilityMatrix[i, 25] = 0.001f;


            //}
            //probabilityMatrix[0, 4] = 1.0f;
            //probabilityMatrix[4, 0] = 0.9f;
        }

        private void SetMatrixValue(double newValue) {

        }

        private void NormalizeProbabilityMatrix() {
            // values cant be below zero or above 1
            // may be redundant
        }
    }
}
