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


        // GENERATION SETTINGS

        //public string currentPreset {
        //    get { return gen.preset.presetName; }
        //    set {
        //        //gen.preset.presetName = value;
        //    }
        //}

        public ObservableCollection<string> genderOptions { get; } = new ObservableCollection<string>() {
            "Male", "Female", "Neutral"
        };
        public int gender {
            get { return (int)gen.gender; }
            set {
                if (gen.SetGender(value)) {
                    OnPropertyChanged(nameof(gender));
                }
            }
        }


        // GENERATION OUTPUT

        public ObservableCollection<string> nameFields { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> historyNames { get; set; } = new ObservableCollection<string>();


        // PRESET GENERAL SETTINGS

        private string _sourcePath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\elven_generic.csv";
        // private string _sourcePath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\human_generic.csv";
        // private string _sourcePath = @"D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\italian.csv";
        // private string _sourcePath = @"nosuchfile";

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


        // STARTING AND ENDING LETTERS

        public ObservableCollection<string> startingLetters { get; } = new ObservableCollection<string>();
        public int selectedStartingLetter {
            get { return gen.selectedStartIndex + 1; }
            set {
                if (gen.SetSelectedIndexStart(value)) {
                    OnPropertyChanged(nameof(selectedStartingLetter));
                }
            }
        }

        public ObservableCollection<string> endingLetters { get; } = new ObservableCollection<string>();
        public int selectedEndingLetter {
            get { return gen.selectedEndIndex + 1; }
            set {
                if (gen.SetSelectedIndexEnd(value)) {
                    OnPropertyChanged(nameof(selectedEndingLetter));
                }
            }
        }


        // COMMANDS

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

            Init();

            generateCommand = new RelayCommand<int>(UpdateNameFields);
            saveNameCommand = new RelayCommand<string>(SaveNameToHistory);
            deleteNameCommand = new RelayCommand<string>(DeleteNameFromHistory);

            minlenDecreaseCommand = new RelayCommand<object>(DecreaseMinLen);
            minlenIncreaseCommand = new RelayCommand<object>(IncreaseMinLen);
            maxlenDecreaseCommand = new RelayCommand<object>(DecreaseMaxLen);
            maxlenIncreaseCommand = new RelayCommand<object>(IncreaseMaxLen);

            //Task.Run(() => {
            //    while (true) {
            //        //if (historyNames.Count > 2) {
            //        //    Debug.WriteLine(": " + historyNames[historyNames.Count - 1].val + " | " + historyNames[historyNames.Count - 2].val);
            //        //}
            //        Debug.WriteLine(": " + gen.selectedStartIndex + " | " + gen.selectedEndIndex);
            //        Thread.Sleep(500);
            //    }
            //});
        }

        private void Init() {
            gen = new Generator(_sourcePath, _presetName);

            startingLetters.Add("any");
            endingLetters.Add("any");

            foreach (Letter letter in gen.preset.alphabet.letters) {
                startingLetters.Add(letter.lowercase.ToString());
                endingLetters.Add(letter.lowercase.ToString());
            }

            for (int i = 0; i < 20; i++) {
                nameFields.Add("");
            }
        }


        // COMMAND ACTIONS

        private void UpdateNameFields(int num) {
            List<string> newnames = new List<string>();
            for (int i = 0; i < num; i++) {
                newnames.Add(gen.GenerateName());
            }

            if (num == 1) {
                for (int i = nameFields.Count - 1; i >= 1; i--) {
                    nameFields[i] = nameFields[i - 1];
                }
                nameFields[0] = newnames[0];
            } else {
                for (int i = 0; i < num; i++) {
                    nameFields.Insert(0, newnames[i]);
                }
            }

            while (nameFields.Count > 20) {
                nameFields.RemoveAt(nameFields.Count - 1);
            }
        }

        private void SaveNameToHistory(string nameToSave) {
            int index = GetHistoryNameAtIndex(nameToSave);
            if (index < 0) {
                historyNames.Add(nameToSave);
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
                if (historyNames[i] == nameToFind) {
                    result = i;
                    break;
                } else if (historyNames[j] == nameToFind) {
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
