using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using Mystic_ToDo.View.UserControls.Content.LoginPage;
using Mystic_ToDo.View.UserControls.Content.Calender;
using Mystic_ToDo.View.UserControls.Content.Reminder;
using Mystic_ToDo.View.UserControls.Content.Time_Tracker;
using Mystic_ToDo.View.UserControls.Content.Timetable;
using Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
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
using static Mystic_ToDo.Database.ReminderDb;
using System.CodeDom;

namespace Mystic_ToDo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ReminderContext ReminderList;
        private int? _selectedReminderId;
        private HomeScreen _homeScreen;
        private RegistrationPage _registrationPage;
        private LoginPage _loginPage;
        private ReminderPage _reminderPage;
        private Calendar _calendar;
        private TimetablePage _timetablePage;
        private TimetrackerPage _timetrackerPage;
        private int _loginId; 

        public int? SelectedReminderId
        {
            get => _selectedReminderId;
            set
            {
                _selectedReminderId = value;
                OnPropertyChanged(nameof(SelectedReminderId));
            }
        }

        public enum SelectedPage
        {
            HomeScreen = 0,
            RegistrationPage = 1,
            login = 2,
            ReminderPage = 3,
            CalenderPage = 4,
            TimetablePage = 5,
            TimetrackerPage = 6
        }

        public int LoginId
        {
            get {  return _loginId; }
            set { _loginId = value; }
        }

        public MainWindow()
        {
            InitializeComponent();

            
            ReminderList = new ReminderContext();
            _reminderPage = new ReminderPage();
            ReminderEditor reminderEditor = (ReminderEditor)FindName("ReminderEditorContent");

            if (reminderEditor != null)
            {
                reminderEditor.SubscribeToReminderPageEvents(_reminderPage);
            }

            OnHomeScreen();

            Menubar.GotoReminderPage += AfterLoginSwitchpage;
            Menubar.GotoCalenderPage += AfterLoginSwitchpage;
            Menubar.GotoTimetablePage += AfterLoginSwitchpage;
            Menubar.GotoTimetrackerPage += AfterLoginSwitchpage;
        }

        private void SetCurrentPage(int selectPage)
        {
            //initialize your pages
            var _homeScreen = new HomeScreen();
            var _registrationPage = new RegistrationPage();
            var _loginPage = new LoginPage();
            var _reminderPage = new ReminderPage();
            var _calenderPage = new CalenderPage();
            var _timetrackerPage = new TimetrackerPage();
            var _timetablePage = new TimetablePage();

            //Clear the current page
            CurrentPage.Children.Clear();

            //cast selectedPage to SelectedPage enum
            SelectedPage selectedPage = (SelectedPage)selectPage;

            switch (selectedPage)
            {
                case SelectedPage.HomeScreen:
                    _homeScreen.ChangetoLoginPage += OnLogin;
                    _homeScreen.ChangetoRegistrationPage += OnRegistration;
                    _homeScreen.ChangetoGuestUser += OnGuestLogin;
                    CurrentPage.Children.Add(_homeScreen);
                    Menubar.UserId = 0;
                    Menubar.Visibility = Visibility.Collapsed;
                    break;
                case SelectedPage.RegistrationPage:
                    _registrationPage.ChangetoHomePage += OnHomeScreen;
                    _registrationPage.RefreshedUserList += OnHomeScreen;
                    CurrentPage.Children.Add(_registrationPage);
                    Menubar.UserId = 0;
                    Menubar.Visibility = Visibility.Collapsed;
                    break;
                case SelectedPage.login:
                    GoToLoginPage(_loginPage, LoginId);
                    break;
                case SelectedPage.ReminderPage:
                    GoToReminderPage(_reminderPage, LoginId);
                    break;
                case SelectedPage.CalenderPage:
                    CurrentPage.Children.Add(_calenderPage);
                    Menubar.Visibility = Visibility.Visible;
                    break;
                case SelectedPage.TimetablePage:
                    CurrentPage.Children.Add(_timetablePage);
                    Menubar.Visibility = Visibility.Visible;
                    break;
                case SelectedPage.TimetrackerPage:
                    CurrentPage.Children.Add(_timetrackerPage);
                    Menubar.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectPage), selectPage, null);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateSelectedReminderIdEvent(int reminderId)
        {
            SelectedReminderId = reminderId;
        }

        public void OnHomeScreen()
        {
            SetCurrentPage(0);
        }

        public void OnLogin(int userId)
        {
            LoginId = userId;
            SetCurrentPage(2);
        }

        public void GoToLoginPage(LoginPage _loginPage, int userId)
        {
            _loginPage.ChangetoHomePage += OnHomeScreen;
            _loginPage.ChangetoReminderPage += AfterLogin;
            _loginPage.UserNumber = userId;
            CurrentPage.Children.Add(_loginPage);
            Menubar.Visibility = Visibility.Collapsed;
        }

        public void AfterLogin (int userId)
        {
            LoginId = userId;
            SetCurrentPage(3);
        }

        public void AfterLoginSwitchpage(int userId, int pageNumber)
        {
            if (userId != 0)
            {
               switch(pageNumber)
                {
                    case 1:
                        SetCurrentPage(3);
                        break;
                    case 2:
                        SetCurrentPage(4);
                        break;
                    case 3:
                        SetCurrentPage(5);
                        break;
                    case 4: 
                        SetCurrentPage(6);
                        break;
                    default:
                        Debug.Write("User not selected");
                        break;
                }
            }
        }

        public void GoToReminderPage (ReminderPage _reminderPage, int userId)
        {
            _reminderPage.UserId = userId;
          /*  _reminderPage.Signout += OnLogout;*/
            Menubar.UserId = userId;
            Menubar.Signout += OnLogout;

            CurrentPage.Children.Add(_reminderPage);
            Menubar.Visibility = Visibility.Visible;
        }


        public void OnGuestLogin(int userId)
        {
            LoginId = userId;
            SetCurrentPage(3);
        }

        public void OnRegistration()
        {
            SetCurrentPage(1);
        }

        public void OnLogout()
        {
            SetCurrentPage(0);
        }
    }
}
