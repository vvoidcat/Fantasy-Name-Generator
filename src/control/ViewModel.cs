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

        public static List<Brush> presetBrushes = new List<Brush> {
            Brushes.AliceBlue,
            Brushes.AntiqueWhite,
            Brushes.PaleVioletRed,
            Brushes.LightSkyBlue,
            Brushes.LightGoldenrodYellow,
            Brushes.White
        };

        private int _selectedBrushIndex = -1;
        public int selectedBrushIndex {
            get {
                int index = 0;
                if (_selectedBrushIndex < 0) {
                    Random rand = new Random();
                    index = rand.Next(0, presetBrushes.Count - 1);
                } else {
                    index = _selectedBrushIndex;
                }
                return index;
            }
            set {
                _selectedBrushIndex = value;
                OnPropertyChanged(nameof(selectedBrushIndex));
            }
        }

        public ObservableCollection<PresetItem> presetItems { get; set; } = new ObservableCollection<PresetItem>() {
            new PresetItem("+", "", true, Brushes.Transparent),
            new PresetItem("Italian", @"D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\italian.csv", false, presetBrushes[1]),
            new PresetItem("Human", @"D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\human_generic.csv", false, presetBrushes[2]),
            new PresetItem("Elven", @"D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\elven_generic.csv", false, presetBrushes[3]),
            new PresetItem("Russian", @"D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\russian.csv", false, presetBrushes[4]),
            new PresetItem("error", @"nosuchfile", false, presetBrushes[0])
        };

        private PresetItem _currentPreset;
        public PresetItem currentPreset {
            get {
                return (_currentPreset is null) ? presetItems[0] : _currentPreset;
            }
            set {
                if (value is not null && gen is not null && gen.SetPreset(value.title, value.filepath)) {
                    _currentPreset = value;
                    OnPropertyChanged(nameof(currentPreset));
                }
            }
        }

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
        public ICommand selectPresetCommand { get; private set; }
        public ICommand deletePresetCommand { get; private set; }

        // unf
        public ICommand addPresetCommand { get; }
        public ICommand saveCommand { get; }
        public ICommand discardCOmmand { get; }
        public ICommand openFinderCommand { get; }
        public ICommand presetLesserCommand { get; }
        public ICommand presetGreaterCommand { get; }


        // CONSTRUCTOR

        public ViewModel() {
            // load saved preset/values if they exist

            Init();

            generateCommand = new RelayCommand<int>(UpdateNameFields);
            saveNameCommand = new RelayCommand<string>(SaveNameToHistory);
            deleteNameCommand = new RelayCommand<string>(DeleteNameFromHistory);

            selectPresetCommand = new RelayCommand<string>(UpdatePresetSelection);
            //addPresetCommand = new RelayCommand
            deletePresetCommand = new RelayCommand<string>(DeletePresetItem);

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
            gen = new Generator(currentPreset.title, currentPreset.filepath);

            startingLetters.Add("any");
            endingLetters.Add("any");

            foreach (Letter letter in gen.alphabet.letters) {
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
            int index = GetListItemAtIndex(nameToSave, historyNames.ToList<string>());
            if (index < 0) {
                historyNames.Add(nameToSave);
            }
        }

        private void DeleteNameFromHistory(string nameToDelete) {
            int index = GetListItemAtIndex(nameToDelete, historyNames.ToList<string>());
            if (index >= 0) {
                historyNames.RemoveAt(index);
            }
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

        private void UpdatePresetSelection(string selectedTitle) {
            PresetItem selectItem = new PresetItem(selectedTitle);

                int index = GetListItemAtIndex(selectItem, presetItems.ToList<PresetItem>());
                if (index >= 0) {
                    for (int i = 0; i < presetItems.Count; i++) {
                        if (i != index) {
                            presetItems[i].isSelected = false;
                        } else {
                            presetItems[i].isSelected = true;
                            currentPreset = presetItems[i];
                        }
                    }
                }
        }

        private void AddPresetItem(string selectedTitle) {
            //
        }

        private void DeletePresetItem(string selectedTitle) {
            PresetItem deleteItem = new PresetItem(selectedTitle);
            int index = GetListItemAtIndex(deleteItem, presetItems.ToList<PresetItem>());
            if (index >= 0) {
                presetItems.RemoveAt(index);
            }
        }


        // HELPERS

        private int GetListItemAtIndex<T>(T objToFind, List<T> list) {
            int i = 0, j = list.Count - 1, result = -100;

            while (i <= j && objToFind is not null) {
                if (objToFind.Equals(list[i])) {
                    result = i;
                    break;
                } else if (objToFind.Equals(list[j])) {
                    result = j;
                    break;
                } else {
                    i++;
                    j--;
                }
            }
            return result;
        }
    }
}
