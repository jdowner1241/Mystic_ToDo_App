using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Mystic_ToDo.View.UserControls.CustomControls
{
    public partial class PasswordBoxWithLabel : UserControl, INotifyPropertyChanged
    {
        public PasswordBoxWithLabel()
        {
            InitializeComponent();
            DataContext = this;
            ShowPasswordToggle = false; // Initialize to false
            ToggleShowPassword(); // Ensure initial state is set correctly
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(PasswordBoxWithLabel), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordBoxWithLabel), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ShowPasswordToggleProperty =
            DependencyProperty.Register("ShowPasswordToggle", typeof(bool), typeof(PasswordBoxWithLabel), new PropertyMetadata(false, OnShowPasswordToggleChanged));

        private static void OnShowPasswordToggleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PasswordBoxWithLabel)d;
            control.ToggleShowPassword();
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set
            {
                SetValue(PlaceholderProperty, value);
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set
            {
                SetValue(PasswordProperty, value);
                OnPropertyChanged();
            }
        }

        public bool ShowPasswordToggle
        {
            get { return (bool)GetValue(ShowPasswordToggleProperty); }
            set
            {
                SetValue(ShowPasswordToggleProperty, value);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            ShowPasswordToggle = !ShowPasswordToggle;
        }

        private void ToggleShowPassword()
        {
            ShowPassword.Visibility = ShowPasswordToggle ? Visibility.Visible : Visibility.Collapsed;

            if (ShowPasswordToggle)
            {
                ShowPassword.Text = PasswordBox.Password;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = PasswordBox.Password;
        }
    }
}
