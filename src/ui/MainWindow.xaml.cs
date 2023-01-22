using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAMEGEN.Core;     // del later
using NAMEGEN.Control;

namespace NAMEGEN.Ui {
    public partial class MainWindow : Window {

        int prevChosenGenderIndex = 0;

        Controller controller;
        private Generator gen;

        private List<TextBlock> nameBlocks;

        public MainWindow() {
            InitializeComponent();

            // load saved progress if exists
            controller = new Controller();
            gen = new Generator(controller.currentPreset);

            InitFields();
        }


        private void InitFields() {
            nameBlocks = new List<TextBlock>() { textName_0, textName_1, textName_2, textName_3, textName_4 };

            foreach (TextBlock name in nameBlocks) {
                name.Text = "";
            }

            panelCustom.Visibility = Visibility.Collapsed;
            bgOverlay.Visibility = Visibility.Collapsed;

            cboxGender.SelectedIndex = (int)controller.gender;
        }

        
        // GENERATION

        private void On_buttonGenerate_Clicked(object sender, RoutedEventArgs e) {
            gen.GenerateName(controller.gender);
            List<string> allNames = gen.GetAllNames();

            if (allNames is not null) {
                for (int i = 0; i < nameBlocks.Count && i < allNames.Count; i++) {
                    nameBlocks[i].Text = gen.GetNameAtIndex(allNames.Count - (1 + i));
                }
            }
        }

        private void On_cboxGender_DropDownOpened(object sender, EventArgs e) {
            prevChosenGenderIndex = cboxGender.SelectedIndex;
        }

        private void On_cboxGender_DropDownClosed(object sender, EventArgs e) {
            int currentIndex = cboxGender.SelectedIndex;
            if (prevChosenGenderIndex != currentIndex && controller is not null) {
                controller.gender = (Gender)currentIndex;
                // disable gender choice selection item if a certain preset matrix is zeroed TODO
            }
        }

        private void On_buttonGenNum_1_Clicked(object sender, RoutedEventArgs e) {
            //
        }

        private void On_buttonGenNum_2_Clicked(object sender, RoutedEventArgs e) {
            //
        }

        private void On_buttonGenNum_3_Clicked(object sender, RoutedEventArgs e) {
            //
        }


        // CONTROL BUTTONS

        private void On_buttonSettings_Clicked(object sender, RoutedEventArgs e) {
            //
        }

        private void On_buttonPresets_Clicked(object sender, RoutedEventArgs e) {
            //
        }

        private void On_buttonCustom_Clicked(object sender, RoutedEventArgs e) {
            if (panelCustom.Visibility == Visibility.Visible) {
                panelCustom.Visibility = Visibility.Collapsed;
                bgOverlay.Visibility = Visibility.Collapsed;
                buttonCustom.Background = Brushes.LightGoldenrodYellow;
                buttonCustom.BorderThickness = new Thickness(1);
            } else {
                panelCustom.Visibility = Visibility.Visible;
                bgOverlay.Visibility = Visibility.Visible;
                buttonCustom.Background = Brushes.Lavender;
                buttonCustom.BorderThickness = new Thickness(1, 1, 1, 0);
            }
        }

        private void On_buttonHistory_Clicked(object sender, RoutedEventArgs e) {
            //
        }


        // SAVE LOAD

        private void On_buttonLoad_Clicked(object sender, RoutedEventArgs e) {
            //
        }

        private void On_buttonSave_Clicked(object sender, RoutedEventArgs e) {
            //
        }

        private void On_buttonSaveas_Clicked(object sender, RoutedEventArgs e) {
            //
        }


        // PRESET GENERAL SETTINGS

        private void On_buttonSourcepath_Clicked(object sender, RoutedEventArgs e) {
            //
        }

        private void On_buttonCoverpath_Clicked(object sender, RoutedEventArgs e) {
            //
        }


        // NAME LENGTH

        private void On_buttonMinLengthLesser_Clicked(object sender, RoutedEventArgs e) {
            int value = Int32.Parse(tboxMinLength.Text);
            int newValue = value - 1;

            if (newValue >= 3) {
                tboxMinLength.Text = newValue.ToString();
                controller.currentPreset.minLength = newValue;
            }
            // deactivate button if value == lower bound
        }

        private void On_buttonMinLengthGreater_Clicked(object sender, RoutedEventArgs e) {
            int value = Int32.Parse(tboxMinLength.Text);
            int newValue = value + 1;

            if (newValue <= 12 && newValue < Int32.Parse(tboxMaxLength.Text)) {
                tboxMinLength.Text = newValue.ToString();
                controller.currentPreset.minLength = newValue;
            }
        }

        private void On_buttonMaxLengthLesser_Clicked(object sender, RoutedEventArgs e) {
            int value = Int32.Parse(tboxMaxLength.Text);
            int newValue = value - 1;

            if (newValue >= 3 && newValue > Int32.Parse(tboxMinLength.Text)) {
                tboxMaxLength.Text = newValue.ToString();
                controller.currentPreset.maxLength = newValue;
            }
            // deactivate button if value == lower bound
        }

        private void On_buttonMaxLengthGreater_Clicked(object sender, RoutedEventArgs e) {
            int value = Int32.Parse(tboxMaxLength.Text);
            int newValue = value + 1;

            if (newValue <= 12) {
                tboxMaxLength.Text = newValue.ToString();
                controller.currentPreset.maxLength = newValue;
            }
        }


        // MAX MIN IN A ROW

        private void On_sbarConsMax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (controller is not null) {
                int value = (int)sbarConsMax.Value;
                tblockConsMaxValue.Text = value.ToString();
                controller.currentPreset.maxRowCons = value;
            }
        }

        private void On_sbarVowsMax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (controller is not null) {
                int value = (int)sbarVowsMax.Value;
                tblockVowsMaxValue.Text = value.ToString();
                controller.currentPreset.maxRowVows = value;
            }
        }


        // PERCENTAGE CORRECTION

        private void On_sbarConsPerc_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (controller is not null) {
                double value = sbarConsPerc.Value;
                tblockConsPercValue.Text = value.ToString();
                controller.currentPreset.conPercentageCorrection = value / 100;
            }
        }

        private void On_sbarVowsPerc_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (controller is not null) {
                double value = sbarVowsPerc.Value;
                tblockVowsPercValue.Text = value.ToString();
                controller.currentPreset.vowPercentageCorrection = value / 100;
            }
        }


        // REPEATS CHECKBOXES

        private void On_checkConsRepeats_Checked(object sender, RoutedEventArgs e) {
            if (checkConsRepeats.IsChecked is not null && controller is not null) {
                controller.currentPreset.allowConsRepeats = (bool)checkConsRepeats.IsChecked;
            }
        }

        private void On_checkVowsRepeats_Checked(object sender, RoutedEventArgs e) {
            if (checkVowsRepeats.IsChecked is not null) {
                controller.currentPreset.allowVowsRepeats = (bool)checkVowsRepeats.IsChecked;
            }
        }


    }
}
