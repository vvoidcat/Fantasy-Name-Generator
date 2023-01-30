using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using NAMEGEN.Core;

namespace NAMEGEN.Control {
    class ViewModel : ObservableObject {
        private Generator gen;

        // APPLICATION SETTINGS

        public List<DataWrapper<Visibility>> panelVisibilities { get; } = new List<DataWrapper<Visibility>> {
            new DataWrapper<Visibility>(Visibility.Collapsed),
            new DataWrapper<Visibility>(Visibility.Collapsed),
            new DataWrapper<Visibility>(Visibility.Collapsed),
            new DataWrapper<Visibility>(Visibility.Collapsed)
        };


        // GENERATION SETTINGS

        //public string currentPreset {
        //    get { return gen.preset.presetName; }
        //    set {
        //        //gen.preset.presetName = value;
        //    }
        //}

        public int gender {
            get { return (int)gen.gender; }
            set {
                if (gen.SetGender(value)) {
                    OnPropertyChanged(nameof(gender));
                }
            }
        }


        // GENERATION OUTPUT

        public List<DataWrapper<string>> nameFields { get; } = new List<DataWrapper<string>> {
            new DataWrapper<string>(""),
            new DataWrapper<string>(""),
            new DataWrapper<string>(""),
            new DataWrapper<string>(""),
            new DataWrapper<string>("")
        };

        public ObservableCollection<DataWrapper<string>> historyNames { get; set; } = new ObservableCollection<DataWrapper<string>>();


        // PRESET GENERAL SETTINGS

        //    string filePath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\elven_generic.csv";
        //    string filePath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\human_generic.csv";
        private string _sourcePath = @"D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\italian.csv";
        // string filePath = @"nosuchfile";

        // upd
        public string sourcePath {
            get { return _sourcePath; }
            set {
                _sourcePath = value;
                gen.preset.SetSourcepath(value);
            }
        }

        // upd
        private string _presetName = "aboba";
        public string presetName {
            get { return _presetName; }
            set {
                _presetName = value; 
                gen.preset.presetName = value;
                OnPropertyChanged(nameof(presetName));
            }
        }


        // MIN MAX NAME LENGTH

        public int minLen {
            get { return gen.minLength; }
            set {
                if (gen.SetMinLength(value)) {
                    OnPropertyChanged(nameof(minLen));
                }
            }
        }

        public int maxLen {
            get { return gen.maxLength; }
            set { 
                if (gen.SetMaxLength(value)) {
                    OnPropertyChanged(nameof(maxLen));
                }
            }
        }


        // VOWS AND CONS

        public double rowLowerbound { get { return Generator.minRow; } }
        public double rowUpperbound { get { return Generator.maxRow; } }
        public double correctionLowerbound { get { return Generator.correctionLowerbound * 100; } }
        public double correctionUpperbound { get { return Generator.correctionUpperbound * 100; } }

        public double consMaxRow {
            get { return gen.maxRowCons; }
            set { 
                if (gen.SetMaxRowCons((int)value)) {
                    OnPropertyChanged(nameof(consMaxRow));
                }
            }
        }

        public double vowsMaxRow {
            get { return gen.maxRowVows; }
            set { 
                if (gen.SetMaxRowVows((int)value)) {
                    OnPropertyChanged(nameof(vowsMaxRow));
                }
            }
        }

        public double consCorrection {
            get { return gen.conPercentageCorrection * 100; }
            set {
                if (gen.SetConCorrection(value)) {
                    OnPropertyChanged(nameof(consCorrection));
                }
            }
        }

        public double vowsCorrection {
            get { return gen.vowPercentageCorrection * 100; }
            set {
                if (gen.SetVowCorrection(value)) {
                    OnPropertyChanged(nameof(vowsCorrection));
                }
            }
        }


        // REPEATS CONTROL

        public bool consAllowRepeats {
            get { return gen.allowConsRepeats; }
            set { 
                if (gen.SetAllowRepeatsCons(value)) {
                    OnPropertyChanged(nameof(consAllowRepeats));
                }
            }
        }

        public bool vowsAllowRepeats {
            get { return gen.allowVowsRepeats; }
            set {
                if (gen.SetAllowRepeatsVows(value)) {
                    OnPropertyChanged(nameof(vowsAllowRepeats));
                }
            }
        }


        // PATTERNS

        public double patternsMax {
            get { return gen.maxPatterns; }
            set {
                if (gen.SetMaxPatterns((int)value)) {
                    OnPropertyChanged(nameof(patternsMax));
                }
            }
        }

        public bool patternsAllowRepeats {
            get { return gen.allowPatternRepeats; }
            set {
                if (gen.SetAllowRepeatsPatterns(value)) {
                    OnPropertyChanged(nameof(patternsAllowRepeats));
                }
            }
        }


        // BUTTON COMMANDS

        public ICommand panelVisibilityCommand { get; private set; }
        public ICommand generateCommand { get; private set; }
        public ICommand saveNameCommand { get; private set; }
        public ICommand deleteNameCommand { get; private set; }
        public ICommand minlenDecreaseCommand { get; private set; }
        public ICommand minlenIncreaseCommand { get; private set; }
        public ICommand maxlenDecreaseCommand { get; private set; }
        public ICommand maxlenIncreaseCommand { get; private set; }

        // unf
        public ICommand deletePresetCommand { get; }
        public ICommand saveCommand { get; }
        public ICommand saveasCommand { get; }
        public ICommand loadCommand { get; }
        public ICommand openSourcepathCommand { get; }
        public ICommand presetLesserCommand { get; }
        public ICommand presetGreaterCommand { get; }


        // CONSTRUCTOR

        public ViewModel() {
            // load saved preset/values if they exist

            gen = new Generator(_sourcePath, _presetName);

            generateCommand = new RelayCommand<object>(UpdateNameFields);
            panelVisibilityCommand = new RelayCommand<string>(UpdateVisibilities);
            saveNameCommand = new RelayCommand<string>(SaveNameToHistory);
            deleteNameCommand = new RelayCommand<string>(DeleteNameFromHistory);

            minlenDecreaseCommand = new RelayCommand<object>(DecreaseMinLen);
            minlenIncreaseCommand = new RelayCommand<object>(IncreaseMinLen);
            maxlenDecreaseCommand = new RelayCommand<object>(DecreaseMaxLen);
            maxlenIncreaseCommand = new RelayCommand<object>(IncreaseMaxLen);

            Task.Run(() => {
                while (true) {
                    //if (historyNames.Count > 2) {
                    //    Debug.WriteLine(": " + historyNames[historyNames.Count - 1].val + " | " + historyNames[historyNames.Count - 2].val);
                    //}
                    Debug.WriteLine(": " + consAllowRepeats + " | " + vowsAllowRepeats + " | " + patternsAllowRepeats + " | " + patternsMax);
                    Thread.Sleep(500);
                }
            });
        }

        // COMMAND ACTIONS

        private void UpdateNameFields(object sender) {
            for (int i = nameFields.Count - 1; i >= 1; i--) {
                nameFields[i].val = nameFields[i - 1].val;
            }
            nameFields[0].val = gen.GenerateName();
        }

        private void UpdateVisibilities(string indexStr) {
            int index = Int32.Parse(indexStr);
            for (int i = 0; i < panelVisibilities.Count; i++) {
                Visibility vis = panelVisibilities[i].val;

                if (i == index && vis != Visibility.Visible) {
                    vis = Visibility.Visible;
                } else {
                    vis = Visibility.Collapsed;
                }

                panelVisibilities[i].val = vis;
            }
        }

        private void SaveNameToHistory(string nameToSave) {
            int index = GetHistoryNameAtIndex(nameToSave);
            if (index < 0) {
                historyNames.Add(new DataWrapper<string>(nameToSave));
            }
        }

        private void DeleteNameFromHistory(string nameToDelete) {
            int index = GetHistoryNameAtIndex(nameToDelete);
            if (index >= 0) {
                historyNames.RemoveAt(index);
            }
        }

        private int GetHistoryNameAtIndex(string nameToFind) {
            int i = 0, j = historyNames.Count - 1, result = -100;

            while (i <= j && !String.IsNullOrEmpty(nameToFind)) {
                if (historyNames[i].val == nameToFind) {
                    result = i;
                    break;
                } else if (historyNames[j].val == nameToFind) {
                    result = j;
                    break;
                } else {
                    i++;
                    j--;
                }
            }
            return result;
        }

        private void DecreaseMinLen(object sender) {
            minLen -= 1;
        }

        private void IncreaseMinLen(object sender) {
            minLen += 1;
        }

        private void DecreaseMaxLen(object sender) {
            maxLen -= 1;
        }

        private void IncreaseMaxLen(object sender) {
            maxLen += 1;
        }
    }
}
