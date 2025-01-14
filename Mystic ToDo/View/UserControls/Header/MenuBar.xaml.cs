﻿using Mystic_ToDo.Data;
using Mystic_ToDo.View.UserControls.CustomControls;
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mystic_ToDo.View.UserControls.Header
{
    /// <summary>
    /// Interaction logic for MenuBar.xaml
    /// </summary>
    public partial class MenuBar : UserControl, INotifyPropertyChanged 
    {
        public MenuBar()
        {
            DataContext = this;
            InitializeComponent();

            
        }

        private int _userId;
        private string _userName;

        private MenuBarButton _lastClickedButton;

        public event Action<int> GotoReminderPage;
        public event Action<int> GotoCalenderPage;
        public event Action<int> GotoTimetablePage;
        public event Action<int> GotoTimetrackerPage;
        public event Action Signout;
        public event PropertyChangedEventHandler PropertyChanged;

        private enum Pagenumber
        {
            Reminderpage = 1,
            Calenderpage = 2,
            Timetable = 3,
            Timetracker = 4
        }

        public int UserId
        {
            get { return _userId; }
            set 
            {
                _userId = value;
                OnPropertyChanged();
                UpdateUserName();
                Debug.Write($"\n\nMenubar with UserID: {UserId} \n\n");
            } 
        }

        public string UserName
        {
           get { return _userName; }
           set {
                _userName = value; 
                OnPropertyChanged(nameof(UserName));
               } 
        }

        private void UpdateUserName()
        {
            using (var db = new ReminderContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserId == UserId); 
                if (user != null) 
                { 
                    UserName = user.UserName; 
                }
                else
                {
                    UserName = string.Empty; // Handle case where user is not found
                } 
            } 
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as MenuBarButton;

            if (_lastClickedButton != null && _lastClickedButton != button)
            { // Reset the last clicked button's color
              _lastClickedButton.Background = Brushes.Silver; 
              _lastClickedButton.BorderBrush = Brushes.Blue;
            }

            if (button != null)
            {
                // Change the clicked button's color
                button.Background = Brushes.LightBlue; 
                button.BorderBrush = Brushes.Blue; 
                
                _lastClickedButton = button;

                switch (button.Name)
                {
                    case "bReminderpage":
                        GotoReminderPage?.Invoke( (int)Pagenumber.Reminderpage);
                        break;
                    case "bCalenderpage":
                        GotoCalenderPage?.Invoke((int)Pagenumber.Calenderpage);
                        break;
                    case "bTimetablepage":
                        GotoTimetablePage?.Invoke((int)Pagenumber.Timetable);
                        break;
                    case "bTimeTrackerpage":
                        GotoTimetrackerPage?.Invoke((int)Pagenumber.Timetracker);
                        break;
                }
            }
        }

        private void bSignOut_Click(object sender, RoutedEventArgs e)
        {
            Signout?.Invoke();
        }
    }
}
