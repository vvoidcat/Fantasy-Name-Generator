using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NAMEGEN.Core;

namespace NAMEGEN.Control {
    class ViewModel {
        private Preset currentPreset { get; set; }

        public ViewModel() {
            // load saved preset/values if they exist

            currentPreset = new Preset(_sourcePath, _lang);

            //Task.Run(() => {
            //    while (true) {
            //        Debug.WriteLine(": " + vowsCorrection + " | ");
            //        Thread.Sleep(500);
            //    }
            //});
        }


        // APPLICATION SETTINGS

        private Language _lang = Language.English;
        public int lang {
            get { return (int)_lang; }
            set { _lang = (Language)value; }
        }


        // GENERATION SETTINGS

        private Gender _gender = Gender.Neutral;
        public int gender {
            get { return (int)_gender; }
            set { _gender = (Gender)value; }
        }


        // BUTTON COMMANDS

        public ICommand GenerateCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SaveasCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand OpenSourcepathCommand { get; }
        public ICommand OpenCoverpathCommand { get; }
        public ICommand MinlenLesserCommand { get; }
        public ICommand MinlenGreaterCommand { get; }
        public ICommand MaxlenLesserCommand { get; }
        public ICommand MaxlenGreaterCommand { get; }
        public ICommand PresetLesserCommand { get; }
        public ICommand PresetGreaterCommand { get; }


        // PRESET GENERAL SETTINGS


        //    string filePath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\elven_generic.csv";
        //    string filePath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\human_generic.csv";
        private string _sourcePath = @"D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\italian.csv";
        // string filePath = @"nosuchfile";

        public string sourcePath {
            get { return _sourcePath; }
            set { _sourcePath = value; }      // update
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
            }
        }

        private double _vowsCorrection = 0.0f;
        public double vowsCorrection {
            get { return _vowsCorrection; }
            set {
                _vowsCorrection = value;
                currentPreset.vowPercentageCorrection = value / 100;
            }
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
