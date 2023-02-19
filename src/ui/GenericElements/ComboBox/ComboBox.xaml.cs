﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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
    /// Interaction logic for ComboBox.xaml
    /// </summary>
    public partial class ComboBox : UserControl {
        public static readonly DependencyProperty BoxWidthProperty =
            DependencyProperty.Register("BoxWidth", typeof(int), typeof(ComboBox), new PropertyMetadata(100));
        public int BoxWidth {
            get { return (int)GetValue(BoxWidthProperty); }
            set { SetValue(BoxWidthProperty, value); }
        }

        public static readonly DependencyProperty ButtonHeightProperty =
            DependencyProperty.Register("ButtonHeight", typeof(int), typeof(ComboBox), new PropertyMetadata(20));
        public int ButtonHeight {
            get { return (int)GetValue(ButtonHeightProperty); }
            set { SetValue(ButtonHeightProperty, value); }
        }

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(int), typeof(ComboBox), new PropertyMetadata(20));
        public int ItemHeight {
            get { return (int)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public static readonly DependencyProperty PopupHeightProperty =
            DependencyProperty.Register("PopupHeight", typeof(int), typeof(ComboBox), new PropertyMetadata(20));
        public int PopupHeight {
            get { return (int)GetValue(PopupHeightProperty); }
            set { SetValue(PopupHeightProperty, value); }
        }

        public static readonly DependencyProperty PopupMaxHeightProperty =
            DependencyProperty.Register("PopupMaxHeight", typeof(int), typeof(ComboBox), new PropertyMetadata(100));
        public int PopupMaxHeight {
            get { return (int)GetValue(PopupMaxHeightProperty); }
            set { SetValue(PopupMaxHeightProperty, value); }
        }

        public static readonly DependencyProperty ItemsCollectionProperty = 
            DependencyProperty.Register("Items", typeof(IEnumerable<string>), typeof(ComboBox), new PropertyMetadata(null));
        public IEnumerable<string> Items {
            get { return (IEnumerable<string>)GetValue(ItemsCollectionProperty); }
            set { SetValue(ItemsCollectionProperty, value); }
        }


        // CONSTRUCTOR

        public ComboBox() {
            Loaded += ControlButton_OnLoaded;

            InitializeComponent();
        }


        // EVENTS

        private void ControlButton_OnLoaded(object sender, EventArgs e) {
            ControlButtonHeader.Width = BoxWidth - ButtonHeight;
            Loaded -= ControlButton_OnLoaded;
        }

        private void ControlButton_CheckedUnchecked(object sender, RoutedEventArgs e) {
            if (ControlButton.IsChecked == true) {
                ArrowDown.Visibility = Visibility.Collapsed;
                ArrowUp.Visibility = Visibility.Visible;
            } else {
                ArrowDown.Visibility = Visibility.Visible;
                ArrowUp.Visibility = Visibility.Collapsed;
            }
        }

        private void ContentList_SizeChanged(object sender, SizeChangedEventArgs e) {
            if (ContentList.Items.Count > 0) {
                PopupHeight = ItemHeight * ContentList.Items.Count + 2;
            } else {
                PopupHeight = ButtonHeight;
            }
        }

        private void ListBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            if (sender as ListBoxItem is not null) {
                if (sender != ContentList.SelectedItem) {
                    ListBoxItem item = (ListBoxItem)sender;
                    ContentList.SelectedItem = item;
                    item.IsSelected = true;
                }
                ComboboxPopup.IsOpen = false;
            }
        }

        //private static void OnItemsCollectionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
        //    var control = (ComboBox)obj;
        //    var oldCollection = e.OldValue as INotifyCollectionChanged;
        //    var newCollection = e.NewValue as INotifyCollectionChanged;

        //    if (oldCollection is not null) {
        //        oldCollection.CollectionChanged -= control.ItemsCollectionChanged;
        //    }

        //    if (newCollection is not null) {
        //        newCollection.CollectionChanged += control.ItemsCollectionChanged;
        //    }

        //    control.UpdateItems();
        //}

        //private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
        //    UpdateItems();
        //}

        //private void UpdateItems() {
        //    Debug.WriteLine("updated");
        //}
    }
}
