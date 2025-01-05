using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using Mystic_ToDo.View.UserControls.Content.LoginPage.LoginPageContent;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Diagnostics;
using System.Linq;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class HomeScreen : UserControl
    {
        private LoginPageUser loginPageUser;
        private ReminderContext reminderContext;

        public event Action<int> ChangetoLoginPage;
        public event Action<int> ChangetoGuestUser;
        public event Action ChangetoRegistrationPage; 

        public HomeScreen()
        {
            DataContext = this;
            InitializeComponent();
            reminderContext = new ReminderContext();

            loadUserList();
            Debug.Write("\n\nHomePage without UserID \n\n");
        }

        //FetchUser List from database
        private List<ReminderDb.User> FetchUsers()
        {
            var renewUserList = new ReminderContext().Users.ToList();
            return renewUserList;
        }

        //Create UI elements from the user list
        private void UpdateUI(IEnumerable<ReminderDb.User> users)
        {
            UserList.Children.Clear();

            foreach (var user in users)
            {
                if (user != null)
                {
                    LoginPageUser loginPageUser = new LoginPageUser();
                    loginPageUser.UserName = user.UserName;
                    loginPageUser.UserNumber = user.UserId;
                    loginPageUser.UserIdSelection += OnUserIdSelection;
                    loginPageUser.RefreshUserList += loadUserList; 

                    UserList.Children.Add(loginPageUser);
                }
            }
        }

        //Load UserList on the Login Page
        private void loadUserList()
        {
            var userList = FetchUsers();
            UpdateUI(userList);
        }

        //Gets the user Id number that was selected
        private void OnUserIdSelection(int selectedUserId)
        {
            if (selectedUserId == 1)
            {
                ChangetoGuestUser?.Invoke(selectedUserId);
            }
            else
            {
                ChangetoLoginPage?.Invoke(selectedUserId);
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            ChangetoRegistrationPage?.Invoke();
        }
    }
}
