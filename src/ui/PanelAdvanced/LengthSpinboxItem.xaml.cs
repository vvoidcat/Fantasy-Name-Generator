using NAMEGEN.Ui;
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
    /// Interaction logic for LengthSpinboxItem.xaml
    /// </summary>
    public partial class LengthSpinboxItem : UserControl {
        public static readonly DependencyProperty titleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(LengthSpinboxItem), new PropertyMetadata(String.Empty));
        public string Title {
            get { return (string)GetValue(titleProperty); }
            set { SetValue(titleProperty, value); }
        }

        public static readonly DependencyProperty boxValueProperty =
            DependencyProperty.Register("BoxValue", typeof(string), typeof(LengthSpinboxItem), new PropertyMetadata(String.Empty));
        public string BoxValue {
            get { return (string)GetValue(boxValueProperty); }
            set { SetValue(boxValueProperty, value); }
        }

        public static readonly DependencyProperty decreaseCommandProperty =
            DependencyProperty.Register("DecreaseCommand", typeof(ICommand), typeof(LengthSpinboxItem), new UIPropertyMetadata(null));
        public ICommand DecreaseCommand {
            get { return (ICommand)GetValue(decreaseCommandProperty); }
            set { SetValue(decreaseCommandProperty, value); }
        }

        public static readonly DependencyProperty increaseCommandProperty =
            DependencyProperty.Register("IncreaseCommand", typeof(ICommand), typeof(LengthSpinboxItem), new UIPropertyMetadata(null));
        public ICommand IncreaseCommand {
            get { return (ICommand)GetValue(increaseCommandProperty); }
            set { SetValue(increaseCommandProperty, value); }
        }

        public LengthSpinboxItem() {
            InitializeComponent();
        }
    }
}
