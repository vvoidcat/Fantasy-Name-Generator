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
        Button[] controlButtons;
        UserControl[] panels;

        public MainWindow() {
            InitializeComponent();
            Init();
        }

        private void Init() {
            controlButtons = new Button[] { buttonPresets, buttonAdvanced, buttonHistory };
            panels = new UserControl[] { panelPresets, panelAdvanced, panelHistory };

            foreach (var panel in panels) {
                panel.Visibility = Visibility.Collapsed;
            }
        }

        private void ControlButton_Click(object sender, RoutedEventArgs e) {
            if (sender is not null && sender is Button) {
                Button thebutton = (Button)sender;

                for (int i = 0; i < controlButtons.Length; i++) {
                    if (controlButtons[i] == thebutton && panels[i].Visibility != Visibility.Visible) {
                        controlButtons[i].BorderThickness = new Thickness(1, 1, 1, 0);
                        panels[i].Visibility = Visibility.Visible;
                    } else {
                        controlButtons[i].BorderThickness = new Thickness(1, 1, 1, 1);
                        panels[i].Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
    }
}
