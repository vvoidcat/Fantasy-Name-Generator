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
        }

        private void On_buttonGenerate_Clicked(object sender, RoutedEventArgs e) {
            gen.GenerateName();
            List<string> allNames = gen.GetAllNames();

            if (allNames is not null) {
                for (int i = 0; i < nameBlocks.Count && i < allNames.Count; i++) {
                    nameBlocks[i].Text = gen.GetNameAtIndex(allNames.Count - (1 + i));
                }
            }
        }

        private void On_buttonSettings_Clicked(object sender, RoutedEventArgs e) {
            //
        }

        private void On_buttonPresets_Clicked(object sender, RoutedEventArgs e) {

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


        private void On_sbarConsMax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (controller is not null) {
                int value = (int)sbarConsMax.Value;
                controller.currentPreset.maxRowCons = value;
                tblockConsMaxValue.Text = value.ToString();
            }
        }

        private void On_sbarVowsMax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (controller is not null) {
                int value = (int)sbarVowsMax.Value;
                controller.currentPreset.maxRowVows = value;
                tblockVowsMaxValue.Text = value.ToString();
            }
        }

        private void On_checkConsRepeats_Checked(object sender, RoutedEventArgs e) {
            //if (checkConsRepeats.IsChecked is not null) {
            //    checkConsRepeats.IsChecked = !checkConsRepeats.IsChecked;
            //    controller.currentPreset.allowConsRepeats = (bool)checkConsRepeats.IsChecked;
            //}
        }

        private void On_checkVowsRepeats_Checked(object sender, RoutedEventArgs e) {
            //if (checkVowsRepeats.IsChecked is not null) {
            //    checkVowsRepeats.IsChecked = !checkVowsRepeats.IsChecked;
            //    controller.currentPreset.allowVowsRepeats = (bool)checkVowsRepeats.IsChecked;
            //}
        }

    }
}
