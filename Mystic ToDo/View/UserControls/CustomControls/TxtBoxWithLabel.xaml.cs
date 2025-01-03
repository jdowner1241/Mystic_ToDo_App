using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Mystic_ToDo.View.UserControls.CustomControls
{
    public partial class TxtBoxWithLabel : UserControl, INotifyPropertyChanged
    {
        public TxtBoxWithLabel()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(TxtBoxWithLabel), new PropertyMetadata(string.Empty));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set
            {
                SetValue(PlaceholderProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty TextValueProperty =
            DependencyProperty.Register("TextValue", typeof(string), typeof(TxtBoxWithLabel), new PropertyMetadata(string.Empty));

        public string TextValue
        {
            get { return (string)GetValue(TextValueProperty); }
            set
            {
                SetValue(TextValueProperty, value);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void txtBoxClear_Click(object sender, RoutedEventArgs e)
        {
            txtBox.Text = string.Empty;
            txtBox.Focus();
        }
    }
}
