﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Mystic_ToDo.View.UserControls.Content.Calender
{
    /// <summary>
    /// Interaction logic for CalenderPage.xaml
    /// </summary>
    public partial class CalenderPage : UserControl, INotifyPropertyChanged
    {

        public CalenderPage()
        {
            DataContext = this;
            InitializeComponent();
        }

        private int _currentUserId;

        public event PropertyChangedEventHandler PropertyChanged;

        public int CurrentUserId
        {
            get => _currentUserId; set
            {
                _currentUserId = value;
                OnPropertyChanged(nameof(CurrentUserId));
                // Perform any action needed when CurrentUserId changes, e.g., loading data for the user
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                //this.DragMove();
            }

        }

        public void lblNote_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtNote.Focus();
        }

        private void txtNote_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNote.Text) && txtNote.Text.Length > 0)
            {
                lblNote.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblNote.Visibility = Visibility.Visible;
            }
        }
        private void lblTime_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtNote.Focus();
        }

        private void txtTime_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTime.Text) && txtTime.Text.Length > 0)

            { lblTime.Visibility = Visibility.Collapsed; }

            else

            { lblTime.Visibility = Visibility.Visible; }
        }
    }
}
