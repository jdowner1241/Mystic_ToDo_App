using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
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
using static Mystic_ToDo.Database.ReminderDb;

namespace Mystic_ToDo.View.UserControls.Content.LoginPage
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : UserControl, INotifyPropertyChanged
    {
        public RegistrationPage()
        {
            DataContext = this;
            InitializeComponent();

            _context = new ReminderContext();
     
        }

        private ReminderContext _context;

        public event PropertyChangedEventHandler PropertyChanged;
        public Action ChangetoHomePage;
        public Action RefreshedUserList;

        private string _name;
        private string _email;
        private string _password;
        private string _password1;
        private string _password2;
        private string _passwordErrorMessage;

        public string UserName 
        {
            get { return _name = txtName.TextValue; }
            set 
            { 
                if (!string.IsNullOrEmpty(value))
                {
                    _name = value;
                }
                else
                {
                    _name = txtName.TextValue;
                }
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _email = txtEmail.EmailTextBox.Text; }
            set 
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _email = value;
                }
                else
                {
                    _email = txtEmail.EmailTextBox.Text;
                }
                OnPropertyChanged();
            }
        }

        public string Password1
        {
            get { return _password1 = txtPassword.PasswordBox.Password; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _password1 = value;
                }else
                {
                    _password1 = txtPassword.PasswordBox.Password;
                }
                OnPropertyChanged();
            }
        }

        public string Password2
        {
            get { return _password2 = txtPassword2.PasswordBox.Password; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _password1 = value;
                }
                else
                {
                    _password2 = txtPassword2.PasswordBox.Password;
                }
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get
            {
                if (Password1 == Password2 && !string.IsNullOrEmpty(Password1)) 
                { 
                    _password = Password1;
                    PasswordErrorMessage = string.Empty; 
                }
                else 
                { 
                    _password = string.Empty;
                    PasswordErrorMessage = "Passwords do not match. Please re-enter."; 
                }
                return _password;
            }
        }

        public string PasswordErrorMessage
        {
            get { return _passwordErrorMessage; }
            private set
            {
                _passwordErrorMessage = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            ChangetoHomePage?.Invoke();
        }

        private void bCreateUser_Click(object sender, RoutedEventArgs e)
        {
            SaveNewUser();
        }

        //Gather Information from Registration page the save the new user
        private void SaveNewUser()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                MessageBox.Show("User Name is required.", "Missing Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(Email))
            {
                MessageBox.Show("Email is required.", "Missing Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Password is required.", "Missing Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = new ReminderContext())
                {
                    UserService.CreateInitialUser(context, UserName, Email, Password);
                    RefreshUserList();
                }

                MessageBox.Show("User created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while creating the user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void RefreshUserList()
        {
            RefreshedUserList?.Invoke();
            ChangetoHomePage?.Invoke();
        }


    }

    
}
