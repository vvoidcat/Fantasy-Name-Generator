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
    /// Interaction logic for LetterControlColumn.xaml
    /// </summary>
    public partial class LetterControlColumn : UserControl {
        public static readonly DependencyProperty titleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(LetterControlColumn), new PropertyMetadata(String.Empty));
        public string Title {
            get { return (string)GetValue(titleProperty); }
            set { SetValue(titleProperty, value); }
        }

        public static readonly DependencyProperty valueRowProperty =
            DependencyProperty.Register("RowValue", typeof(double), typeof(LetterControlColumn), new PropertyMetadata(0.0));
        public double RowValue {
            get { return (double)GetValue(valueRowProperty); }
            set { SetValue(valueRowProperty, value); }
        }

        public static readonly DependencyProperty valueCorrectionProperty =
            DependencyProperty.Register("CorrectionValue", typeof(double), typeof(LetterControlColumn), new PropertyMetadata(0.0));
        public double CorrectionValue {
            get { return (double)GetValue(valueCorrectionProperty); }
            set { SetValue(valueCorrectionProperty, value); }
        }

        public static readonly DependencyProperty allowAllRepeatsProperty =
            DependencyProperty.Register("AllowAllRepeats", typeof(bool), typeof(LetterControlColumn), new PropertyMetadata(true));
        public bool AllowAllRepeats {
            get { return (bool)GetValue(allowAllRepeatsProperty); }
            set { SetValue(allowAllRepeatsProperty, value); }
        }

        public LetterControlColumn() {
            InitializeComponent();
        }
    }
}
