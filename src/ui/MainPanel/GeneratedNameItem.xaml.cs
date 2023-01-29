﻿using System;
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

        public GeneratedNameItem() {
            InitializeComponent();
        }
    }
}
