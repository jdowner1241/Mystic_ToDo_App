using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Mystic_ToDo.View.UserControls.CustomControls
{
    /// <summary>
    /// Interaction logic for EmailBoxWithLabel.xaml
    /// </summary>
    public partial class EmailBoxWithLabel : UserControl, INotifyPropertyChanged
    {
        public EmailBoxWithLabel()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(EmailBoxWithLabel), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register("Email", typeof(string), typeof(EmailBoxWithLabel), new PropertyMetadata(string.Empty, OnEmailChanged));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set
            {
                SetValue(PlaceholderProperty, value);
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return (string)GetValue(EmailProperty); }
            set
            {
                SetValue(EmailProperty, value);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static void OnEmailChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (EmailBoxWithLabel)d;
            control.ValidateEmail();
        }

        private void ValidateEmail()
        {
            var emailValidationRule = new EmailValidationRule();
            var validationResult = emailValidationRule.Validate(Email, CultureInfo.CurrentCulture);
            if (!validationResult.IsValid)
            {
                // Handle invalid email format (e.g., show a message to the user)
                MessageBox.Show(validationResult.ErrorContent.ToString(), "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

    public class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Email is required.");
            }

            string email = value.ToString();
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (Regex.IsMatch(email, pattern))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Invalid email format.");
            }
        }
    }
}
