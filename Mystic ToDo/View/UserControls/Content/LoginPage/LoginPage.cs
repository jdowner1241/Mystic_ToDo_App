using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using Mystic_ToDo.View.UserControls.Content.LoginPage.LoginPageContent;
using System;
using System.Collections.Generic;
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

        public HomeScreen()
        {
            DataContext = this;
            InitializeComponent();
            reminderContext = new ReminderContext();

            loadUserList();
        }

        //FetchUser List
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
    }
}
