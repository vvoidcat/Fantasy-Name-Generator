using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
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
        private Object[] nameitemContainers;

        public static readonly DependencyProperty SelectedNumValueProperty =
            DependencyProperty.Register("SelectedNumValue", typeof(int), typeof(GenerationControls), new PropertyMetadata(1));
        public int SelectedNumValue {
            get { return (int)GetValue(SelectedNumValueProperty); }
            set { SetValue(SelectedNumValueProperty, value); }
        }

        public GenerationControls() {
            InitializeComponent();
            Init();
        }

        private void Init() {
            numControls = new ToggleButton[] { buttonGenNum_1, buttonGenNum_10, buttonGenNum_20 };
            nameitemContainers = new Object[] { containerGenNum_1, containerGenNum_10, containerGenNum_20 };
        }

        private void ButtonNumControl_Checked(object sender, RoutedEventArgs e) {
            if (sender is not null && sender is ToggleButton && numControls is not null) {
                ToggleButton tbutton = (ToggleButton)sender;

                for (int i = 0; i < numControls.Length; i++) {
                    if (tbutton != numControls[i]) {
                        numControls[i].IsChecked = false;
                        UpdateContainerVisibility(i, Visibility.Collapsed);
                    } else {
                        if (Int32.TryParse(numControls[i].Content.ToString(), out int res)) {
                            SelectedNumValue = res;
                        } else {
                            SelectedNumValue = 1;
                        }
                        UpdateContainerVisibility(i, Visibility.Visible);
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

        private void UpdateContainerVisibility(int index, Visibility newVisState) {
            if (nameitemContainers[index] is StackPanel) {
                StackPanel container = (StackPanel)nameitemContainers[index];
                container.Visibility = newVisState;
            } else if (nameitemContainers[index] is Grid) {
                Grid container = (Grid)nameitemContainers[index];
                container.Visibility = newVisState;
            }
        }
    }
}
