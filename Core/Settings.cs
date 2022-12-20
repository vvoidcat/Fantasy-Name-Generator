using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NAMEGEN.Core.Settings;

namespace NAMEGEN.Core {
    public class Settings {
        public int minLength { get; private set; }
        public int maxLength { get; private set; }

        public int maxRowVows { get; private set; }
        public int maxRowCons { get; private set; }

        public double vowPercentageCorrection { get; private set; }
        public double conPercentageCorrection { get; private set; }

        public Gender gender { get; private set; }
        public Alphabet alphabet { get; private set; }
        public double[,] probabilityMatrix { get; private set; }


        public Settings() {
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

            // open file with
            // calculate and store probability matrix
            InitProbabilityMatrix();
            UpdateProbabilityMatrix();
        }

        public void UpdateSettings() {

        }

        private void InitProbabilityMatrix() {
            probabilityMatrix = new double[alphabet.lettersCount, alphabet.lettersCount];

            for (int i = 0; i < alphabet.lettersCount; i++) {
                for (int j = 0; j < alphabet.lettersCount; j++) {
                    probabilityMatrix[i, j] = 0.2f;     // change later
                }
            }
        }

        private void UpdateProbabilityMatrix() {
            // need to know letters exact pos
            // need to take the file with names as an arg to parse it

            //probabilityMatrix[0, 4] = 0.9f;
            //probabilityMatrix[0, 4] = 0.9f;

            for (int i = 0; i < alphabet.lettersCount; i++) {
                probabilityMatrix[0, i] = 0.001f;
                probabilityMatrix[4, i] = 0.001f;

                probabilityMatrix[22, i] = 0.001f;
                probabilityMatrix[i, 22] = 0.001f;

                probabilityMatrix[23, i] = 0.001f;
                probabilityMatrix[i, 23] = 0.001f;

                probabilityMatrix[25, i] = 0.001f;
                probabilityMatrix[i, 25] = 0.001f;


            }
            probabilityMatrix[0, 4] = 0.9f;
            probabilityMatrix[4, 0] = 0.9f;
        }


    }
}
