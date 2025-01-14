﻿using Mystic_ToDo.Data;
using Mystic_ToDo.View.UserControls.CustomControls;
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

namespace Mystic_ToDo.View.UserControls.Content.LoginPage
{
    /// <summary>
    /// Interaction logic for LoginPage1.xaml
    /// </summary>
    public partial class LoginPage : UserControl, INotifyPropertyChanged
    {
    
        public LoginPage()
        {
            DataContext = this;
            InitializeComponent();

            _context = new ReminderContext();
        }

        private ReminderContext _context;
        private string _userName;
        private int _userNumber;
        private string _email;
        private string _password;

        public event PropertyChangedEventHandler PropertyChanged;
        public Action ChangetoHomePage;
        public Action<int> ChangetoReminderPage;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
                UpdateUserNameFromDatebase();
            }
        }

        public int UserNumber
        {
            get { return _userNumber; }
            set
            {
                _userNumber = value;
                OnPropertyChanged();
                UpdateUserNameFromDatebase();
            }
        }

        public string Email
        {
            get { return _email = EmailBox.EmailTextBox.Text; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _email = value;
                }
                else
                {
                    _email = EmailBox.EmailTextBox.Text;
                }
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return _password = PasswordBox.PasswordBox.Password; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _password = value;
                }
                else
                {
                    _password = PasswordBox.PasswordBox.Password;
                }
                OnPropertyChanged();
            }
        }

        private void UpdateUserNameFromDatebase()
        {
            using (var db = new ReminderContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserId == UserNumber);
                if (user != null)
                {
                    _userName = user.UserName;
                    OnPropertyChanged(nameof(UserName));    
                }
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

        private void bLogin_Click(object sender, RoutedEventArgs e)
        {
            ValidationCheck();
        }

        private void ValidationCheck()
        {
            using (var db = new ReminderContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserId == UserNumber);
                if (user != null)
                {
                   if (!string.IsNullOrEmpty(Email) && Email == user.EmailAddress)
                    {
                        if(!string.IsNullOrEmpty(Password) && Password == user.Password)
                        {
                           ChangetoReminderPage?.Invoke(UserNumber);
                        }
                        else
                        {
                            MessageBox.Show("Email or Password incorrect.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Email or Password incorrect.");
                    }
                }
            }
        }
    }
}
