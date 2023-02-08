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

namespace NAMEGEN.Ui
{
    /// <summary>
    /// Interaction logic for LetterControlField.xaml
    /// </summary>
    public partial class LetterControlField : UserControl {
        public static readonly DependencyProperty titleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(LetterControlField), new PropertyMetadata(String.Empty));
        public string Title {
            get { return (string)GetValue(titleProperty); }
            set { SetValue(titleProperty, value); }
        }

        public static readonly DependencyProperty valueRowProperty =
            DependencyProperty.Register("RowValue", typeof(double), typeof(LetterControlField), new PropertyMetadata(0.0));
        public double RowValue {
            get { return (double)GetValue(valueRowProperty); }
            set { SetValue(valueRowProperty, value); }
        }

        public static readonly DependencyProperty allowRepeatsProperty =
            DependencyProperty.Register("AllowRepeats", typeof(bool), typeof(LetterControlField), new PropertyMetadata(true));
        public bool AllowRepeats {
            get { return (bool)GetValue(allowRepeatsProperty); }
            set { SetValue(allowRepeatsProperty, value); }
        }

        public LetterControlField() {
            InitializeComponent();
        }
    }
}
