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

namespace NAMEGEN.Ui {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
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
