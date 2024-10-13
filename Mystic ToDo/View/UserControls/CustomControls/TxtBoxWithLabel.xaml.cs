using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Mystic_ToDo.View.UserControls.CustomControls
{
    /// <summary>
    /// Interaction logic for txtBoxWithLabel.xaml
    /// </summary>
    public partial class TxtBoxWithLabel : UserControl, INotifyPropertyChanged
    {
        public TxtBoxWithLabel()
        {
            DataContext = this;
            InitializeComponent();
        }

        private string placeholder;
        private string textValue;

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

        public string TextValue
        {
            get { return textValue; }
            set
            {
                textValue = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void txtBoxClear_Click(object sender, RoutedEventArgs e)
        {
            //if(txtBox = string.IsNullOrEmpty)
            txtBox.Text = string.Empty;
            txtBox.Focus();
        }
    }
}
