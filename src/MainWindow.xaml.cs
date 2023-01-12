using NAMEGEN.Core;
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

namespace fantasy_namegen {
    public partial class MainWindow : Window {

        //    string filepath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\elven_generic.csv";
        //    string filepath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\human_generic.csv";
        string filepath = @"D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\source-tables\\italian.csv";
        //string filepath = @"nosuchfile";
        Language lang = NAMEGEN.Core.Language.English;
        Gender gender = Gender.Neutral;

        public Preset currentPreset { get; private set; }
        private Generator gen;

        private List<TextBlock> nameBlocks;

        public MainWindow() {
            InitializeComponent();

            // load saved progress if exists

            currentPreset = new Preset(filepath, lang);
            gen = new Generator(currentPreset);

            InitFields();
        }


        private void InitFields() {
            nameBlocks = new List<TextBlock>() { textName_0, textName_1, textName_2, textName_3, textName_4 };

            foreach (TextBlock name in nameBlocks) {
                name.Text = "";
            }
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
            } else {
                panelCustom.Visibility = Visibility.Visible;
                bgOverlay.Visibility = Visibility.Visible;
            }
        }

        private void On_buttonHistory_Clicked(object sender, RoutedEventArgs e) {
            //if (panelCustom.Visibility == Visibility.Visible) {
            //    panelCustom.Visibility = Visibility.Collapsed;
            //    bgOverlay.Visibility = Visibility.Collapsed;
            //} else {
            //    panelCustom.Visibility = Visibility.Visible;
            //    bgOverlay.Visibility = Visibility.Visible;
            //}
        }
    }
}
