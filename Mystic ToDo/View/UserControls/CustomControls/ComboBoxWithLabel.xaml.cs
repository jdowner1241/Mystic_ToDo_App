﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Mystic_ToDo.View.UserControls.CustomControls
{
    /// <summary>
    /// Interaction logic for ComboBoxWithLabel.xaml
    /// </summary>
    public partial class ComboBoxWithLabel : UserControl, INotifyPropertyChanged
    {
        public ComboBoxWithLabel()
        {
            DataContext = this;
            InitializeComponent();
        }


        private string placeholder;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Placeholder
        {
            get { return placeholder; }
            set
            {
                placeholder = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName= null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
