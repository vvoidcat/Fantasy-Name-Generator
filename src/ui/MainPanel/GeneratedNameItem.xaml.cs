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
    /// <summary>
    /// Interaction logic for GeneratedNameItem.xaml
    /// </summary>
    public partial class GeneratedNameItem : UserControl {
        public static readonly DependencyProperty titleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(GeneratedNameItem), new PropertyMetadata(string.Empty));
        public string Title {
            get { return (string)GetValue(titleProperty); }
            set { SetValue(titleProperty, value); }
        }

        public static readonly DependencyProperty ButtonSizeProperty =
            DependencyProperty.Register("ButtonSize", typeof(int), typeof(GeneratedNameItem), new PropertyMetadata(20));
        public int ButtonSize {
            get { return (int)GetValue(ButtonSizeProperty); }
            set { SetValue(ButtonSizeProperty, value); }
        }

        public static readonly DependencyProperty ItemFontSizeProperty =
            DependencyProperty.Register("ItemFontSize", typeof(int), typeof(GeneratedNameItem), new PropertyMetadata(20));
        public int ItemFontSize {
            get { return (int)GetValue(ItemFontSizeProperty); }
            set { SetValue(ItemFontSizeProperty, value); }
        }

        public GeneratedNameItem() {
            InitializeComponent();
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e) {
            addButton.Visibility = Visibility.Collapsed;

        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e) {
            if (!String.IsNullOrEmpty(Title)) {
                addButton.Visibility = Visibility.Visible;
            }
        }
    }
}
