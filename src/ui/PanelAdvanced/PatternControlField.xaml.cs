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
    /// Interaction logic for PatternControlField.xaml
    /// </summary>
    public partial class PatternControlField : UserControl {
        public static readonly DependencyProperty valueMaxProperty =
            DependencyProperty.Register("PatternsCountValue", typeof(double), typeof(PatternControlField), new PropertyMetadata(0.0));
        public double PatternsCountValue {
            get { return (double)GetValue(valueMaxProperty); }
            set { SetValue(valueMaxProperty, value); }
        }

        public PatternControlField() {
            InitializeComponent();
        }
    }
}
