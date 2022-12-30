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

        //    string filepath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\elven_generic.csv";
        //    string filepath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\human_generic.csv";
        string filepath = "D:\\FUCKING CODE\\Fantasy-Name-Generator\\materials\\italian.csv";
        Language lang = NAMEGEN.Core.Language.English;

        public Settings currentPreset { get; private set; }
        private Generator gen;

        private List<TextBlock> nameBlocks;

        public MainWindow() {
            InitializeComponent();

            // load saved progress if exists

            currentPreset = new Settings(filepath, lang);
            gen = new Generator(currentPreset);

            InitFields();
        }


        private void InitFields() {
            nameBlocks = new List<TextBlock>() { textName_0, textName_1, textName_2, textName_3, textName_4 };

            foreach (TextBlock name in nameBlocks) {
                name.Text = "";
            }
        }

        private void On_buttonGenerate_Clicked(object sender, EventArgs eventArgs) {
            gen.GenerateName();

            if (gen.GetAllNames() is not null) {
                for (int i = 0; i < 5 && i < gen.GetAllNames().Count; i++) {
                    nameBlocks[i].Text = gen.GetNameAtIndex(i);
                }
            }
        }
    }
}
