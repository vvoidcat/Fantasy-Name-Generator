using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NAMEGEN.Ui {
    /// <summary>
    /// Interaction logic for GenerationControls.xaml
    /// </summary>
    public partial class GenerationControls : UserControl {
        private ToggleButton[] numControls;

        public GenerationControls() {
            InitializeComponent();
            Init();
        }

        private void Init() {
            numControls = new ToggleButton[] { buttonGenNum_1, buttonGenNum_5, buttonGenNum_15 };
        }

        private void ButtonNumControl_Checked(object sender, RoutedEventArgs e) {
            if (sender is not null && sender is ToggleButton && numControls is not null) {
                ToggleButton tbutton = (ToggleButton)sender;

                for (int i = 0; i < numControls.Length; i++) {
                    if (tbutton != numControls[i]) {
                        numControls[i].IsChecked = false;
                    } else if (tbutton == numControls[i] && tbutton.IsChecked == false) {
                        numControls[i].IsChecked = true;
                    }
                }
            }
        }

        private void ButtonNumControl_Unchecked(object sender, RoutedEventArgs e) {
            if (sender is not null && sender is ToggleButton && numControls is not null) {
                ToggleButton tbutton = (ToggleButton)sender;
                int countChecks = 0;
                int indexChecked = 0;

                for (int i = 0; i < numControls.Length; i++) {
                    if (numControls[i].IsChecked == true) {
                        countChecks++;
                    }
                    if (tbutton == numControls[i]) {
                        indexChecked = i;
                    }
                }

                if (countChecks == 0) {
                    numControls[indexChecked].IsChecked = true;
                }
            }
        }
    }
}
