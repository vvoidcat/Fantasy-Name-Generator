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
        private GenerationSettings genSettings;


        // APPLICATION SETTINGS

        public List<DataWrapper<Visibility>> panelVisibilities { get; } = new List<DataWrapper<Visibility>> {
            new DataWrapper<Visibility>(Visibility.Collapsed),
            new DataWrapper<Visibility>(Visibility.Collapsed),
            new DataWrapper<Visibility>(Visibility.Collapsed),
            new DataWrapper<Visibility>(Visibility.Collapsed)
        };


        // GENERATION SETTINGS

        //public string currentPreset {
        //    get { return genSettings.preset.presetName; }
        //    set {
        //        //genSettings.preset.presetName = value;
        //    }
        //}

        public int gender {
            get { return (int)genSettings.gender; }
            set {
                if (genSettings.SetGender(value)) {
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
                genSettings.preset.SetSourcepath(value);
            }
        }

        // upd
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

        public int minLen {
            get { return genSettings.minLength; }
            set {
                if (genSettings.SetMinLength(value)) {
                    OnPropertyChanged(nameof(minLen));
                }
            }
        }

        public int maxLen {
            get { return genSettings.maxLength; }
            set { 
                if (genSettings.SetMaxLength(value)) {
                    OnPropertyChanged(nameof(maxLen));
                }
            }
        }


        // VOWS AND CONS

        public double rowLowerbound { get { return GenerationSettings.minRow; } }
        public double rowUpperbound { get { return GenerationSettings.maxRow; } }
        public double correctionLowerbound { get { return GenerationSettings.correctionLowerbound * 100; } }
        public double correctionUpperbound { get { return GenerationSettings.correctionUpperbound * 100; } }

        public double consMaxRow {
            get { return genSettings.maxRowCons; }
            set { 
                if (genSettings.SetMaxRowCons((int)value)) {
                    OnPropertyChanged(nameof(consMaxRow));
                }
            }
        }

        public double vowsMaxRow {
            get { return genSettings.maxRowVows; }
            set { 
                if (genSettings.SetMaxRowVows((int)value)) {
                    OnPropertyChanged(nameof(vowsMaxRow));
                }
            }
        }

        public double consCorrection {
            get { return genSettings.conPercentageCorrection * 100; }
            set {
                if (genSettings.SetConCorrection(value)) {
                    OnPropertyChanged(nameof(consCorrection));
                }
            }
        }

        public double vowsCorrection {
            get { return genSettings.vowPercentageCorrection * 100; }
            set {
                if (genSettings.SetVowCorrection(value)) {
                    OnPropertyChanged(nameof(vowsCorrection));
                }
            }
        }


        // REPEATS CONTROL

        public bool consAllowRepeats {
            get { return genSettings.allowConsRepeats; }
            set { 
                if (genSettings.SetAllowRepeatsCons(value)) {
                    OnPropertyChanged(nameof(consAllowRepeats));
                }
            }
        }

        public bool vowsAllowRepeats {
            get { return genSettings.allowVowsRepeats; }
            set {
                if (genSettings.SetAllowRepeatsVows(value)) {
                    OnPropertyChanged(nameof(vowsAllowRepeats));
                }
            }
        }


        // BUTTON COMMANDS

        public ICommand panelVisibilityCommand { get; set; }
        public ICommand generateCommand { get; set; }
        public ICommand deleteNameCommand { get; }

        // unf
        public ICommand deletePresetCommand { get; }
        public ICommand saveCommand { get; }
        public ICommand saveasCommand { get; }
        public ICommand loadCommand { get; }
        public ICommand openSourcepathCommand { get; }
        public ICommand minlenLesserCommand { get; }
        public ICommand minlenGreaterCommand { get; }
        public ICommand maxlenLesserCommand { get; }
        public ICommand maxlenGreaterCommand { get; }
        public ICommand presetLesserCommand { get; }
        public ICommand presetGreaterCommand { get; }


        // CONSTRUCTOR

        public ViewModel() {
            // load saved preset/values if they exist

            genSettings = new GenerationSettings(_sourcePath, _presetName, true);
            gen = new Generator(genSettings);

            generateCommand = new RelayCommand<object>(UpdateNameFields);
            panelVisibilityCommand = new RelayCommand<string>(UpdateVisibilities);
            deleteNameCommand = new RelayCommand<object>(DeleteNameFromHistory);

            Task.Run(() => {
                while (true) {
                    //if (historyNames.Count > 2) {
                    //    Debug.WriteLine(": " + historyNames[historyNames.Count - 1].val + " | " + historyNames[historyNames.Count - 2].val);
                    //}
                    Debug.WriteLine(": " + historyNames.Count);
                    Thread.Sleep(500);
                }
            });
        }

        // COMMAND ACTIONS

        private void UpdateNameFields(object sender) {
            gen.GenerateName();
            List<string> allNames = gen.GetAllNames();

            if (allNames is not null) {
                for (int i = 0; i < nameFields.Count && i < allNames.Count; i++) {
                    nameFields[i].val = gen.GetNameAtIndex(allNames.Count - (1 + i));
                }
            }

            historyNames.Add(new DataWrapper<string>(gen.GetLastName()));
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

        private void DeleteNameFromHistory(object nameToDelete) {
            historyNames.Clear();


            //for (int i = 0; i < historyNames.Count; i++) {
            //    if (historyNames[i].val == nameToDelete) {
            //        //historyNames.RemoveAt(i);
            //        historyNames.Clear();
            //        break;
            //    }
            //}
        }
    }
}


// CommandParameter = "{Binding Title, RelativeSource={RelativeSource AncestorType=UserControl}}"
