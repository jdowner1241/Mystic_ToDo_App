using System;
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

namespace Mystic_ToDo.View.UserControls.CustomControls
{
    /// <summary>
    /// Interaction logic for PasswordBoxWithLabel.xaml
    /// </summary>
    public partial class PasswordBoxWithLabel : UserControl, INotifyPropertyChanged
    {
        public PasswordBoxWithLabel()
        {
            DataContext = this;
            InitializeComponent();

            ShowPasswordToggle = false;
        }

        private string placeholder;
        private bool _showPasswordToggle;
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

        public bool ShowPasswordToggle 
        { get 
            {
                return _showPasswordToggle; 
            } 
            set { 
                _showPasswordToggle = value; 
                OnPropertyChanged(); 
                ToggleShowPassword(); 
            } 
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            ShowPasswordToggle = !ShowPasswordToggle;

            if (ShowPasswordToggle) 
            { 
                string password = PasswordBox.Password; 
                ShowPassword.Text = password; 
            }
        }

        private void ToggleShowPassword()
        {
            {
                ShowPassword.Visibility = ShowPasswordToggle ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
