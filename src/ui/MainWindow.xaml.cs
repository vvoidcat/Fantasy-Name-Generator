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

namespace NAMEGEN.Ui {
    public partial class MainWindow : Window {

        private Generator gen;
        private List<TextBlock> nameBlocks;

        public MainWindow() {
            InitializeComponent();

            Preset preset = new Preset("aaa", Core.Language.English);
            gen = new Generator(preset);

            InitFields();
        }


        private void InitFields() {
            nameBlocks = new List<TextBlock>() { textName_0, textName_1, textName_2, textName_3, textName_4 };

            foreach (TextBlock name in nameBlocks) {
                name.Text = "";
            }

            panelCustom.Visibility = Visibility.Collapsed;
            bgOverlay.Visibility = Visibility.Collapsed;

            //cboxGender.SelectedIndex = (int)controller.gender;
        }

        
        // GENERATION

        private void On_buttonGenerate_Clicked(object sender, RoutedEventArgs e) {
            gen.GenerateName(Gender.Neutral);
            List<string> allNames = gen.GetAllNames();

            if (allNames is not null) {
                for (int i = 0; i < nameBlocks.Count && i < allNames.Count; i++) {
                    nameBlocks[i].Text = gen.GetNameAtIndex(allNames.Count - (1 + i));
                }
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

        // NAME LENGTH

        private void On_buttonMinLengthLesser_Clicked(object sender, RoutedEventArgs e) {
            //int value = Int32.Parse(tboxMinLength.Text);
            //int newValue = value - 1;

            //if (newValue >= 3) {
            //    tboxMinLength.Text = newValue.ToString();
            //    controller.currentPreset.minLength = newValue;
            //}
            // deactivate button if value == lower bound
        }

        private void On_buttonMinLengthGreater_Clicked(object sender, RoutedEventArgs e) {
            //int value = Int32.Parse(tboxMinLength.Text);
            //int newValue = value + 1;

            //if (newValue <= 12 && newValue < Int32.Parse(tboxMaxLength.Text)) {
            //    tboxMinLength.Text = newValue.ToString();
            //    controller.currentPreset.minLength = newValue;
            //}
        }

        private void On_buttonMaxLengthLesser_Clicked(object sender, RoutedEventArgs e) {
            //int value = Int32.Parse(tboxMaxLength.Text);
            //int newValue = value - 1;

            //if (newValue >= 3 && newValue > Int32.Parse(tboxMinLength.Text)) {
            //    tboxMaxLength.Text = newValue.ToString();
            //    controller.currentPreset.maxLength = newValue;
            //}
            // deactivate button if value == lower bound
        }

        private void On_buttonMaxLengthGreater_Clicked(object sender, RoutedEventArgs e) {
            //int value = Int32.Parse(tboxMaxLength.Text);
            //int newValue = value + 1;

            //if (newValue <= 12) {
            //    tboxMaxLength.Text = newValue.ToString();
            //    controller.currentPreset.maxLength = newValue;
            //}
        }
    }
}
