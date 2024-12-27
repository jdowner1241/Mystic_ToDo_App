using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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

namespace Mystic_ToDo.View.UserControls.Content.LoginPage
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : UserControl, INotifyPropertyChanged
    {
        private ReminderContext _context;

        public event PropertyChangedEventHandler PropertyChanged;
        public Action ChangetoHomePage;
        public Action RefreshedUserList;

        private string _name;
        private string _email;
        private string _password;

        public RegistrationPage()
        {
            DataContext = this;
            InitializeComponent();

            _context = new ReminderContext();  
        }

        public string _Name 
        {
            get { return _name; }
            set 
            { 
                _name = txtName.TextValue;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _email; }
            set 
            { 
                _email = txtEmail.EmailTextBox.Text;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set 
            { 
                var password1 = txtPassword.PasswordBox.Password;
                var password2 = txtPassword2.PasswordBox.Password;
                if (password1 == password2)
                {
                    _password = password1;
                }
                else
                {
                    MessageBox.Show("Passwords are not the same. Please reconform the entered password.");
                }
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
            //var user = GetInfoFromPage();
            SavetoDatabase();
        }

        //Gather Information from Registration page
        private ReminderDb.User GetInfoFromPage()
        {
            ReminderDb.User userInfo = new ReminderDb.User();

            if (!string.IsNullOrEmpty(txtName.TextValue))
            {
                userInfo.UserName = txtName.TextValue;
            }

            if (!string.IsNullOrEmpty(txtEmail.EmailTextBox.Text))
            {
                userInfo.EmailAddress = txtEmail.EmailTextBox.Text;
            }

            var password1 = txtPassword.PasswordBox.Password;
            var password2 = txtPassword2.PasswordBox.Password;
            if (!string.IsNullOrEmpty(password1) && !string.IsNullOrEmpty(password2))
            {
                if (password1 == password2)
                {
                    userInfo.Password = password1;
                }
                else
                {
                    MessageBox.Show("Passwords are not the same. Please reconform the entered password.");
                }
            }
            else
            {
                MessageBox.Show("Passwords are yet reconfirmed. Please reconform the entered password.");
            }
            return userInfo;

        }

        //Save info to the Database
        private void SavetoDatabase()
        {
            ReminderDb.User addUser = GetInfoFromPage();
            var existingUser = _context.Users.FirstOrDefault(r => r.UserName == addUser.UserName);

            if (existingUser != null)
            {
                // Handle case where a reminder with the same name already exists
                System.Windows.MessageBox.Show("A User with this name already exists.");
                return;
            }

            _context.SaveUser(addUser);
            RefreshUserList();
        }

        private void RefreshUserList()
        {
            RefreshedUserList?.Invoke();
            ChangetoHomePage?.Invoke();
        }


    }

    
}
