using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using NAMEGEN.Core;

namespace NAMEGEN.Control {
    class ViewModel : ObservableObject {
        private Generator gen;
        private GenerationSettings genSettings;


        // APPLICATION SETTINGS

        private Language _lang = Language.English;
        public int lang {
            get { return (int)_lang; }
            set { _lang = (Language)value; }
        }




        // GENERATION SETTINGS

        //private Preset _currentPreset = genSettings.preset;
        //public Preset currentPreset {
        //    get { return _currentPreset; }
        //    set {
        //        _currentPreset = value;
        //        genSettings.preset = value;
        //    }
        //}

        private Gender _gender = Gender.Neutral;
        public int gender {
            get { return (int)_gender; }
            set {
                _gender = (Gender)value;
                genSettings.gender = _gender;
            }
        }


        // GENERATION OUTPUT

        public List<Wrapper<string>> nameFields { get; } = new List<Wrapper<string>> {
            new Wrapper<string>(""),
            new Wrapper<string>(""),
            new Wrapper<string>(""),
            new Wrapper<string>(""),
            new Wrapper<string>("")
        };


        // PRESET GENERAL SETTINGS

        //    string filePath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\elven_generic.csv";
        //    string filePath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\human_generic.csv";
        private string _sourcePath = @"D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\italian.csv";
        // string filePath = @"nosuchfile";

        public string sourcePath {
            get { return _sourcePath; }
            set {
                _sourcePath = value;
                genSettings.preset.SetSourcepath(value);
            }
        }

        private string _coverPath = "default";
        public string coverPath {
            get { return _coverPath; }
            set {
                _coverPath = value;
                OnPropertyChanged(nameof(coverPath));
            }
        }

        private string _presetName = "aboba";
        public string presetName {
            get { return _presetName; }
            set {
                _presetName = value; 
                genSettings.preset.presetName = value;
                OnPropertyChanged(nameof(presetName));
            }
        }


        // MIN MAX NAME LENGTH

        private int _minLen = 3;
        public int minLen {
            get { return _minLen; }
            set { 
                _minLen = value;
                genSettings.minLength = value;        // needs a setter
            }
        }

        private int _maxLen = 12;
        public int maxLen {
            get { return _maxLen; }
            set { 
                _maxLen = value;
                genSettings.maxLength = value;
                // OnPropertyChanged()
            }
        }


        // MAX VOWS AND CONS IN A ROW

        private double _consMaxRow = 2;
        public double consMaxRow {
            get { return _consMaxRow; }
            set { 
                _consMaxRow = value; 
                genSettings.maxRowCons = (int)value;      // needs a setter
                OnPropertyChanged(nameof(consMaxRow));
            }
        }

        private double _vowsMaxRow = 2;
        public double vowsMaxRow {
            get { return _vowsMaxRow; }
            set { 
                _vowsMaxRow = value;
                genSettings.maxRowVows = (int)value;
                OnPropertyChanged(nameof(vowsMaxRow));
            }
        }

        private double _consCorrection = 0.0f;
        public double consCorrection {
            get { return _consCorrection; }
            set {
                _consCorrection = value;
                genSettings.conPercentageCorrection = value / 100;
                OnPropertyChanged(nameof(consCorrection));
            }
        }

        private double _vowsCorrection = 0.0f;
        public double vowsCorrection {
            get { return _vowsCorrection; }
            set {
                _vowsCorrection = value;
                genSettings.vowPercentageCorrection = value / 100;
                OnPropertyChanged(nameof(vowsCorrection));
            }
        }


        // REPEATS CONTROL

        private bool _consAllowRepeats = true;
        public bool consAllowRepeats {
            get { return _consAllowRepeats; }
            set { 
                _consAllowRepeats = value; 
                genSettings.allowConsRepeats = value;
                OnPropertyChanged(nameof(consAllowRepeats));
            }
        }

        private bool _vowsAllowRepeats = true;
        public bool vowsAllowRepeats {
            get { return _vowsAllowRepeats; }
            set {
                _vowsAllowRepeats = value; 
                genSettings.allowVowsRepeats = value; 
                OnPropertyChanged(nameof(vowsAllowRepeats)); 
            }
        }


        // BUTTON COMMANDS

        public ICommand generateCommand { get; set; }
        public ICommand saveCommand { get; }
        public ICommand saveasCommand { get; }
        public ICommand loadCommand { get; }
        public ICommand openSourcepathCommand { get; }
        public ICommand openCoverpathCommand { get; }
        public ICommand minlenLesserCommand { get; }
        public ICommand minlenGreaterCommand { get; }
        public ICommand maxlenLesserCommand { get; }
        public ICommand maxlenGreaterCommand { get; }
        public ICommand presetLesserCommand { get; }
        public ICommand presetGreaterCommand { get; }


        // CONSTRUCTOR

        public ViewModel() {
            // load saved preset/values if they exist

            genSettings = new GenerationSettings(_sourcePath, _coverPath, _presetName, _lang, true);
            gen = new Generator(genSettings);

            this.generateCommand = new RelayCommand(UpdateNameFields);

            Task.Run(() => {
                while (true) {
                    Debug.WriteLine(": " + genSettings.preset.sourcepath + " | " + _sourcePath);
                    Thread.Sleep(500);
                }
            });
        }

        // COMMAND ACTIONS

        private void UpdateNameFields() {
            gen.GenerateName();
            List<string> allNames = gen.GetAllNames();

            if (allNames is not null) {
                for (int i = 0; i < nameFields.Count && i < allNames.Count; i++) {
                    nameFields[i].val = gen.GetNameAtIndex(allNames.Count - (1 + i));
                }
            }
        }
    }
}
