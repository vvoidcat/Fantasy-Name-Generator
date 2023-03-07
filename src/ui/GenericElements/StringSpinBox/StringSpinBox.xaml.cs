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
    /// Interaction logic for StringSpinBox.xaml
    /// </summary>
    public partial class StringSpinBox : UserControl {
        public static readonly DependencyProperty BoxWidthProperty =
            DependencyProperty.Register("BoxWidth", typeof(int), typeof(StringSpinBox), new PropertyMetadata(100));
        public int BoxWidth {
            get { return (int)GetValue(BoxWidthProperty); }
            set { SetValue(BoxWidthProperty, value); }
        }

        public static readonly DependencyProperty BoxHeightProperty =
            DependencyProperty.Register("BoxHeight", typeof(int), typeof(StringSpinBox), new PropertyMetadata(20));
        public int BoxHeight {
            get { return (int)GetValue(BoxHeightProperty); }
            set { SetValue(BoxHeightProperty, value); }
        }

        public static readonly DependencyProperty boxValueProperty =
            DependencyProperty.Register("BoxValue", typeof(string), typeof(StringSpinBox), new PropertyMetadata(String.Empty));
        public string BoxValue {
            get { return (string)GetValue(boxValueProperty); }
            set { SetValue(boxValueProperty, value); }
        }

        public static readonly DependencyProperty decreaseCommandProperty =
            DependencyProperty.Register("DecreaseCommand", typeof(ICommand), typeof(StringSpinBox), new UIPropertyMetadata(null));
        public ICommand DecreaseCommand {
            get { return (ICommand)GetValue(decreaseCommandProperty); }
            set { SetValue(decreaseCommandProperty, value); }
        }

        public static readonly DependencyProperty increaseCommandProperty =
            DependencyProperty.Register("IncreaseCommand", typeof(ICommand), typeof(StringSpinBox), new UIPropertyMetadata(null));
        public ICommand IncreaseCommand {
            get { return (ICommand)GetValue(increaseCommandProperty); }
            set { SetValue(increaseCommandProperty, value); }
        }

        public StringSpinBox() {
            InitializeComponent();
        }
    }
}
