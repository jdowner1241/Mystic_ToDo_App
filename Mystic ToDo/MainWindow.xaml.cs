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
using Mystic_ToDo.View.UserControls.Header;

namespace Mystic_ToDo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();


            ReminderList = new ReminderContext();

            OnHomeScreen();

            MenubarUI.GotoReminderPage += AfterLoginSwitchpage;
            MenubarUI.GotoCalenderPage += AfterLoginSwitchpage;
            MenubarUI.GotoTimetablePage += AfterLoginSwitchpage;
            MenubarUI.GotoTimetrackerPage += AfterLoginSwitchpage;

            Debug.Write($"\n\nMainPage with CurrentUserID: {CurrentLoginId} \n\n");
            Debug.Write($"\n\nMainPage with SelectedUserID: {SelectedLoginId} \n\n");
        }


        private ReminderContext ReminderList;
        private int? _selectedReminderId;
        private HomeScreen _homeScreen;
        private RegistrationPage _registrationPage;
        private LoginPage _loginPage;
        private ReminderPage _reminderPage;
        private CalenderPage _calendarPage;
        private TimetablePage _timetablePage;
        private TimetrackerPage _timetrackerPage;
        private int _CurrentloginId; 
        private int _SelectedloginId; 

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

        public int CurrentLoginId
        {
            get => _CurrentloginId;
            set 
            {
                _CurrentloginId = value;
                OnPropertyChanged(nameof(_CurrentloginId)); 
                Debug.Write($"\n\nLoginId set to: {_CurrentloginId}\n\n");
            }
        }

        public int SelectedLoginId
        {
            get => _SelectedloginId;
            set
            {
                _SelectedloginId = value;
                OnPropertyChanged(nameof(_SelectedloginId));
                Debug.Write($"\n\nLoginId set to: {_SelectedloginId}\n\n");
            }
        }

        private void SetCurrentPage(int selectPage)
        {
            //initialize your pages
            // Initialize pages if they are null (reuse existing instances)
            if (_homeScreen == null) _homeScreen = new HomeScreen(); 
            if (_registrationPage == null) _registrationPage = new RegistrationPage(); 
            if (_loginPage == null) _loginPage = new LoginPage(); 
            if (_reminderPage == null) _reminderPage = new ReminderPage(); 
            if (_calendarPage == null) _calendarPage = new CalenderPage(); 
            if (_timetrackerPage == null) _timetrackerPage = new TimetrackerPage(); 
            if (_timetablePage == null) _timetablePage = new TimetablePage();

            //Clear the current page
            CurrentPage.Children.Clear();

            //cast selectedPage to SelectedPage enum
            SelectedPage selectedPage = (SelectedPage)selectPage;

            switch (selectedPage)
            {
                case SelectedPage.HomeScreen:
                    _homeScreen.ChangetoLoginPage += OnLoginPage;
                    _homeScreen.ChangetoRegistrationPage += OnRegistration;
                    _homeScreen.ChangetoGuestUser += OnGuestLogin;
                    CurrentPage.Children.Add(_homeScreen);
                    MenubarUI.Visibility = Visibility.Collapsed;
                    break;
                case SelectedPage.RegistrationPage:
                    _registrationPage.ChangetoHomePage += OnHomeScreen;
                    _registrationPage.RefreshedUserList += OnHomeScreen;
                    CurrentPage.Children.Add(_registrationPage);
                    MenubarUI.Visibility = Visibility.Collapsed;
                    break;
                case SelectedPage.login:
                    GoToLoginPage(_loginPage);
                    break;
                case SelectedPage.ReminderPage:
                    GoToReminderPage(_reminderPage);
                    MenubarUI.Visibility = Visibility.Visible;
                    break;
                case SelectedPage.CalenderPage:
                    _calendarPage.CurrentUserId = CurrentLoginId;
                    CurrentPage.Children.Add(_calendarPage);
                    MenubarUI.Visibility = Visibility.Visible;
                    break;
                case SelectedPage.TimetablePage:
                    _timetablePage.CurrentUserId = CurrentLoginId;
                    CurrentPage.Children.Add(_timetablePage);
                    MenubarUI.Visibility = Visibility.Visible;
                    break;
                case SelectedPage.TimetrackerPage:
                    _timetrackerPage.CurrentUserId = CurrentLoginId;
                    CurrentPage.Children.Add(_timetrackerPage);
                    MenubarUI.Visibility = Visibility.Visible;
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

        public void OnLoginPage(int userId)
        {
            SelectedLoginId = userId;
            SetCurrentPage(2);
        }

        public void GoToLoginPage(LoginPage _loginPage)
        {
            _loginPage.ChangetoHomePage += OnHomeScreen;
            _loginPage.ChangetoReminderPage += AfterLogin;
            _loginPage.UserNumber = SelectedLoginId;
            CurrentPage.Children.Add(_loginPage);
            MenubarUI.Visibility = Visibility.Collapsed;
        }

        public void AfterLogin (int userId)
        {
            CurrentLoginId = userId;
            UpdateMenubarId(CurrentLoginId);
            SetCurrentPage(3);
        }

        public void AfterLoginSwitchpage(int pageNumber)
        {
               switch(pageNumber)
                {
                    case 1:
                        SetCurrentPage((int)SelectedPage.ReminderPage); 
                        break;
                    case 2:
                        _calendarPage.CurrentUserId = CurrentLoginId; 
                        SetCurrentPage((int)SelectedPage.CalenderPage);
                        break;
                    case 3:
                        _timetablePage.CurrentUserId = CurrentLoginId; 
                        SetCurrentPage((int)SelectedPage.TimetablePage);
                        break;
                    case 4:
                        _timetrackerPage.CurrentUserId = CurrentLoginId; 
                        SetCurrentPage((int)SelectedPage.TimetrackerPage);
                        break;
                    default:
                        Debug.Write("User not selected");
                        break;
                }
        }

        public void GoToReminderPage(ReminderPage _reminderPage)
        {
            MenubarUI.Visibility = Visibility.Visible; 

            _reminderPage.UserId = CurrentLoginId;
            CurrentPage.Children.Add(_reminderPage);

            Dispatcher.InvokeAsync(() => 
            {
                ReminderEditor reminderEditor = (ReminderEditor)FindName("ReminderEditorContent"); 
            
                if (reminderEditor != null) 
                {
                    reminderEditor.CurrentUserId = CurrentLoginId;
                    reminderEditor.SubscribeToReminderPageEvents(_reminderPage); 
                }

                PersonalFolder1 personalFolder1 = (PersonalFolder1)FindName("PersonalFolder"); 
                if (personalFolder1 != null) 
                { 
                    personalFolder1.UserId = CurrentLoginId;
                    personalFolder1.LoadFolderList(); 
                }
            
                Filter1 filter1 = (Filter1)FindName("FilterContent"); if (filter1 != null)
                { 
                    // Ensure Filter1 is set up correctly if needed
                } 
            });

        }


        public void OnGuestLogin(int userId)
        {
            CurrentLoginId = userId;
            UpdateMenubarId(userId);
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

        public void UpdateMenubarId(int currentUserId)
        {
            MenubarUI.UserId = currentUserId;
            MenubarUI.Signout += OnLogout;
        }
    }
}
