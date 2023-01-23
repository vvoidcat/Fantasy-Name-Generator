using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NAMEGEN.Core;

namespace NAMEGEN.Control {
    class Controller : ObservableObject {
        //    string filepath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\elven_generic.csv";
        //    string filepath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\human_generic.csv";
        //string filepath = @"D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\italian.csv";
        // string filepath = @"nosuchfile";

        Language lang = Language.English;
        public Gender gender = Gender.Neutral;

        private Preset currentPreset { get; set; }
        private Generator gen;

        public Controller() {
            // load saved preset/values if they exist

            currentPreset = new Preset(filePath, lang);
            gen = new Generator(currentPreset);

            Task.Run(() => {
                while (true) {
                    Debug.WriteLine(": " + vowsCorrection + " | " + currentPreset.presetName);
                    Thread.Sleep(500);
                }
            });
        }


        // PRESET GENERAL SETTINGS

        private string _filePath = @"D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\italian.csv";
        public string filePath {
            get { return _filePath; }
            set { _filePath = value; }      // update
        }

        private string _coverPath = "default";
        public string coverPath {
            get { return _coverPath; }
            set { _coverPath = value; }
        }

        private string _presetName = "aboba";
        public string presetName {
            get { return _presetName; }
            set { _presetName = value; currentPreset.presetName = value; }
        }


        // MIN MAX NAME LENGTH

        private int _minLen = 3;
        public int minLen {
            get { return _minLen; }
            set { _minLen = value; currentPreset.minLength = value; }       // needs a setter
        }

        private int _maxLen = 12;
        public int maxLen {
            get { return _maxLen; }
            set { _maxLen = value; currentPreset.maxLength = value; }
        }


        // MAX VOWS AND CONS IN A ROW

        private double _consMaxRow = 2;
        public double consMaxRow {
            get { return _consMaxRow; }
            set { _consMaxRow = value; currentPreset.maxRowCons = (int)value; }    // needs a setter
        }

        private double _vowsMaxRow = 2;
        public double vowsMaxRow {
            get { return _vowsMaxRow; }
            set { _vowsMaxRow = value; currentPreset.maxRowVows = (int)value; }
        }

        private double _consCorrection = 0.0f;
        public double consCorrection {
            get { return _consCorrection; }
            set {
                _consCorrection = value;
                currentPreset.conPercentageCorrection = value / 100;
                OnPropertyChanged(consCorrectionTblock);
                //_consCorrectionTblock = ((int)_consCorrection).ToString() + "%"; 
            }
        }

        private double _vowsCorrection = 0.0f;
        public double vowsCorrection {
            get { return _vowsCorrection; }
            set {
                _vowsCorrection = value;
                currentPreset.vowPercentageCorrection = value / 100;
                _vowsCorrectionTblock = ((int)_consCorrection).ToString() + "%";
            }
        }

        private string _consCorrectionTblock = "0%";
        public string consCorrectionTblock {
            get { return _consCorrectionTblock; }
            set { _consCorrectionTblock = ((int)_consCorrection).ToString() + "%"; }
        }

        private string _vowsCorrectionTblock = "0%";
        public string vowsCorrectionTblock {
            get { return _vowsCorrectionTblock; }
        }


        // REPEATS CONTROL

        private bool _consAllowRepeats = true;
        public bool consAllowRepeats {
            get { return _consAllowRepeats; }
            set { _consAllowRepeats = value; currentPreset.allowConsRepeats = value; }
        }

        private bool _vowsAllowRepeats = true;
        public bool vowsAllowRepeats {
            get { return _vowsAllowRepeats; }
            set { _vowsAllowRepeats = value; currentPreset.allowVowsRepeats = value; }
        }

    }
}
