using Mystic_ToDo.Data;
using Mystic_ToDo.View.UserControls.Content.Reminder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mystic_ToDo.View.UserControls.Content.LoginPage.LoginPageContent
{
    /// <summary>
    /// Interaction logic for LoginPageUser.xaml
    /// </summary>
    public partial class LoginPageUser : UserControl, INotifyPropertyChanged
    {
        public LoginPageUser()
        {
            DataContext = this;
            InitializeComponent();

            this.MouseLeftButtonDown += this.OnMouseLeftButtonDown;
        }

        private string username;
        private int userNumber;
        public event PropertyChangedEventHandler? PropertyChanged;

        public event Action<int> UserIdSelection;
        public event Action RefreshUserList;

        public string UserName
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        public int UserNumber
        {
            get { return userNumber; }
            set { 
                userNumber = value; 
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public Brush SetBackground
        {
            get { return (Brush)GetValue(SetBackgroundProperty); }
            set { SetValue(SetBackgroundProperty, value); }
        }

        public static readonly DependencyProperty SetBackgroundProperty =
            DependencyProperty.Register("SetBackground", typeof(Brush), typeof(LoginPageUser), new PropertyMetadata(Brushes.LightGray));

        private void OnMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            UserIdSelection?.Invoke(userNumber);
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            DeleteUser.Foreground = Brushes.Yellow;

            MessageBoxResult result = System.Windows.MessageBox.Show(
                $" Do you want to Delete this User? \n User Name: {UserName}",
                "Delete User !!!",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.OK) 
            {
                using (var dbContext = new ReminderContext())
                {
                    var selectedUser = dbContext.Users.SingleOrDefault(r => r.UserId == userNumber);

                    if (selectedUser != null)
                    {
                        System.Windows.MessageBox.Show($"User Removed!!! \n\nUser ID: {userNumber} \nReminder Name: {username}");
                        dbContext.Users.Remove(selectedUser);

                        dbContext.SaveChanges();
                        OnRefreshUserList();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Reminder not found");
                    }
                }
            }
            else
            {

            }
        }

        private void OnRefreshUserList()
        {
            RefreshUserList?.Invoke();
        }

        private void DeleteUser_MouseEnter(object sender, MouseEventArgs e)
        {
            DeleteUser.Background = Brushes.Red;
        }

        private void DeleteUser_MouseLeave(object sender, MouseEventArgs e)
        {
            DeleteUser.Background = Brushes.LightGray;
        }

        private void background_MouseEnter(object sender, MouseEventArgs e)
        {
            background.Background = Brushes.LightBlue;
        }

        private void background_MouseLeave(object sender, MouseEventArgs e)
        {
            background.Background = Brushes.LightGray;
        }

        private void DeleteUser_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
