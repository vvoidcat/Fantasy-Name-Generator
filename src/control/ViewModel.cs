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
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using NAMEGEN.Core;

namespace NAMEGEN.Control {
    class ViewModel : ObservableObject {
        private Generator gen;
        private int addCounter = 1;


        // GENERATION SETTINGS

        public static List<Brush> presetBrushes { get; } = new List<Brush> {
            Brushes.White,
            Brushes.LightGray,
            Brushes.Gray,
            Brushes.PaleVioletRed,
            Brushes.LightCoral,
            Brushes.LightPink,
            Brushes.Orange,
            Brushes.LightGoldenrodYellow,
            Brushes.CornflowerBlue,
            Brushes.LightSkyBlue,
            Brushes.LightCyan,
            Brushes.LightSeaGreen,
            Brushes.LightGreen,
        };

        private Brush _selectedBrush = presetBrushes[0];
        public Brush selectedBrush {
            get { return _selectedBrush; }
            set {
                _selectedBrush = value;
                OnPropertyChanged(nameof(selectedBrush));
            }
        }

        public ObservableCollection<PresetItem> presetItems { get; set; } = new ObservableCollection<PresetItem>() {
            new PresetItem("+", "", false, null),
            new PresetItem("Italian", @"../../../materials/source-tables/italian.csv", true, presetBrushes),
            new PresetItem("Human", @"../../../materials/source-tables/human_generic.csv", true, presetBrushes),
            new PresetItem("Elven", @"../../../materials/source-tables/elven_generic.csv", true, presetBrushes),
            new PresetItem("Russian", @"../../../materials/source-tables/russian.csv", true, presetBrushes),
            new PresetItem("error", @"nosuchfile", true, presetBrushes)
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

        private int _currentPresetIndex = 0;
        private int currentPresetIndex {
            get {
                return _currentPresetIndex;
            }
            set {
                if (value >= 0 && value < presetItems.Count) {
                    _currentPresetIndex = value;
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
        public ICommand presetLesserCommand { get; private set; }
        public ICommand presetGreaterCommand { get; private set; }
        public ICommand saveCommand { get; }
        public ICommand discardCOmmand { get; }
        public ICommand openFinderCommand { get; }


        // CONSTRUCTOR

        public ViewModel() {
            // load saved preset/values if they exist

            Init();

            generateCommand = new RelayCommand<int>(UpdateNameFields);
            saveNameCommand = new RelayCommand<string>(SaveNameToHistory);
            deleteNameCommand = new RelayCommand<string>(DeleteNameFromHistory);

            selectPresetCommand = new RelayCommand<string>(UpdatePresetSelection);
            deletePresetCommand = new RelayCommand<string>(DeletePresetItem);
            presetLesserCommand = new RelayCommand<string>(DecreasePreset);
            presetGreaterCommand = new RelayCommand<string>(IncreasePreset);

            minlenDecreaseCommand = new RelayCommand<object>(DecreaseMinLen);
            minlenIncreaseCommand = new RelayCommand<object>(IncreaseMinLen);
            maxlenDecreaseCommand = new RelayCommand<object>(DecreaseMaxLen);
            maxlenIncreaseCommand = new RelayCommand<object>(IncreaseMaxLen);

            //Task.Run(() => {
            //    while (true) {
            //        //if (historyNames.Count > 2) {
            //        //    Debug.WriteLine(": " + historyNames[historyNames.Count - 1].val + " | " + historyNames[historyNames.Count - 2].val);
            //        //}
            //        Debug.WriteLine(": " + currentPresetIndex + " | " + selectedBrush.ToString());
            //        Thread.Sleep(500);
            //    }
            //});
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
                if (!presetItems[index].isDeletable) {
                    presetItems[index].isSelected = false;
                    AddPresetItem();
                } else {
                    for (int i = 0; i < presetItems.Count; i++) {
                        if (i != index) {
                            presetItems[i].isSelected = false;
                        } else {
                            presetItems[i].isSelected = true;
                            currentPreset = presetItems[i];
                            currentPresetIndex = i;
                            selectedBrush = presetItems[i].color;
                        }
                    }
                }
            }
        }

        private void DeletePresetItem(string selectedTitle) {
            PresetItem deleteItem = new PresetItem(selectedTitle);
            int index = GetListItemAtIndex(deleteItem, presetItems.ToList<PresetItem>());

            if (index >= 0) {
                presetItems.RemoveAt(index);

                if (currentPresetIndex == index) {
                    if (presetItems.Count > index) {
                        UpdatePresetSelection(presetItems[index].title);
                    } else if (presetItems.Count == index) {
                        DecreasePreset(null);
                    }
                }
            }
            if (presetItems.Count == 1) {
                currentPresetIndex = 0;
                currentPreset = presetItems[0];

            }
        }

        private void DecreasePreset(object? sender) {
            currentPresetIndex -= 1;

            if (currentPresetIndex > 0) { 
                UpdatePresetSelection(presetItems[currentPresetIndex].title);
            }
        }

        private void IncreasePreset(object? sender) {
            currentPresetIndex += 1;

            if (currentPresetIndex > 0) {
                UpdatePresetSelection(presetItems[currentPresetIndex].title);
            }
        }


        // HELPERS

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

        private void AddPresetItem() {
            while (presetItems.Contains(new PresetItem("New Preset " + addCounter))) {
                addCounter++;
            }
            presetItems.Add(new PresetItem("New Preset " + addCounter, "choose a path", true, presetBrushes));
            addCounter++;
        }

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
