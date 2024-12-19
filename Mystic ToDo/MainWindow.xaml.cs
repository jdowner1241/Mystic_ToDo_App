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

namespace Mystic_ToDo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ReminderContext ReminderList;
        private int? _selectedReminderId;
        private LoginPage _loginPage;
        private ReminderPage _reminderPage;
        private Calendar _calendar;
        private TimetablePage _timetablePage;
        private TimetrackerPage _timetrackerPage;

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
            login = 0,
            ReminderPage = 1,
            CalenderPage = 2,
            TimetrackerPage = 3,
            TimetablePage = 4
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

        }

        private void SetCurrentPage(int selectPage)
        {
            //initialize your pages
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
                case SelectedPage.login:
                    CurrentPage.Children.Add(_loginPage);
                    Menubar.Visibility = Visibility.Collapsed;
                    break;
                case SelectedPage.ReminderPage:
                    CurrentPage.Children.Add(_reminderPage);
                    Menubar.Visibility = Visibility.Visible;
                    break;
                case SelectedPage.CalenderPage:
                    CurrentPage.Children.Add(_calenderPage);
                    Menubar.Visibility = Visibility.Visible;
                    break;
                case SelectedPage.TimetrackerPage:
                    CurrentPage.Children.Add(_timetrackerPage);
                    Menubar.Visibility = Visibility.Visible;
                    break;
                case SelectedPage.TimetablePage:
                    CurrentPage.Children.Add(_timetablePage);
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

        public void OnLogin()
        {
            SetCurrentPage(0);
        }

        public void OnLogout()
        {
            SetCurrentPage(0);
        }
    }
}
