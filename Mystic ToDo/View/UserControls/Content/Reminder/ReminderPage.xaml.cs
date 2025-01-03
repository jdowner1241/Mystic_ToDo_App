﻿using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using static Mystic_ToDo.Database.ReminderDb;

namespace Mystic_ToDo.View.UserControls.Content.Reminder
{
    /// <summary>
    /// Interaction logic for ReminderPage.xaml
    /// </summary>
    public partial class ReminderPage : UserControl, INotifyPropertyChanged
    {

        public ReminderPage()
        {
            InitializeComponent();
            DataContext = this;

            DbContext = new ReminderContext();
            //LoadDataFromReminderPage();

            var reminderEditor = (ReminderEditor)FindName("ReminderEditorContent");
            if (reminderEditor != null)
            {
                reminderEditor.SubscribeToReminderPageEvents(this);
                reminderEditor.CurrentUserId = UserId;
                reminderEditor.CurrentFolderId = CurrentFolderId;
            }
            var filter = (Filter1)FindName("FilterContent");
            if (filter != null)
            {
                filter.SubscribeToReminderPageEvents(this);
            }

            LoadPersonalFolders();
            /*var personalFolder = (PersonalFolder1)FindName("PersonalFolder");
            if (personalFolder != null) 
            {
                
            }*/

        }

        private readonly ReminderContext DbContext;
        private bool _isUpdating = false;
        private string searchValue;
        private int _userId;
        private int _currentFolderId;
        private string _userName;
        private TaskList CurrentTaskList { get; set; }
        private TaskList SearchedTaskList { get; set; }

        public static readonly DependencyProperty SelectedReminderIdProperty = DependencyProperty.Register("SelectedReminderId", typeof(int?), typeof(ReminderPage), new PropertyMetadata(null, OnSelectedReminderChanged));
        public static readonly DependencyProperty SelectedReminderIdListProperty = DependencyProperty.Register("SelectedReminderIdList", typeof(List<int?>), typeof(ReminderPage), new PropertyMetadata(null, OnSelectedReminderListChanged));

        public event EventHandler ReminderChanged;
        public event Action<int?> SelectedReminderIdChanged;
        public event EventHandler ReminderListChanged;
        public event Action<List<int?>> SelectedReminderListChanged;
        public event Action<string> ReminderPageSearchValueChanged;
        public event Action Signout;
        public event PropertyChangedEventHandler PropertyChanged;

        public int? SelectedReminderId
        {
            get => (int?)GetValue(SelectedReminderIdProperty);
            set => SetValue(SelectedReminderIdProperty, value);
        }

        public List<int?> SelectedReminderIdList
        {
            get => (List<int?>)GetValue(SelectedReminderIdListProperty);
            set => SetValue(SelectedReminderIdListProperty, value);
        }

        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get 
            {
                using (var db = new ReminderContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.UserId == UserId);
                    if (user != null)
                    {
                        _userName = user.UserName; 
                    }
                }
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public int CurrentFolderId
        {
            get { return _currentFolderId; }
            set
            {
                _currentFolderId = value;
                OnPropertyChanged();
            }
        }

        public string SearchValue
        {
            get => searchValue;
            set
            {
                searchValue = value;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        // Fetch reminders from the database for a specific user ID
        private List<ReminderDb.Reminder> FetchReminders()
        {
            if (UserId != 0)
            {
                using (var context = new ReminderContext())
                {
                    var reminders = context.Reminders
                                            .Where(reminder => reminder.UserId == UserId)
                                            .ToList();
                    return reminders;
                }
            }
            else
            {
                Debug.WriteLine("User not set");
                return new List<ReminderDb.Reminder>(); // Return an empty list to handle the case where UserId is not set
            }
        }


        // Fetch searched value from the database
        private List<ReminderDb.Reminder> FetchSearchValue(string searchValue)
        {
            var reminderList = FetchReminders(); // Retrieve reminders based on UserId

            // Filter the retrieved reminders based on the search value
            var searchResults = reminderList
                                .Where(r => r.Name != null && r.Name.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                            r.Description != null && r.Description.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                            r.SelectedFolder != null && r.SelectedFolder.FolderName != null && r.SelectedFolder.FolderName.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                            r.SelectedUser != null && r.SelectedUser.UserName != null && r.SelectedUser.UserName.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                                .ToList();

            return searchResults;
        }


        // Event that gets and run the search method
        public void ReminderPageSearch(ReminderDb.Reminder reminder)
        {
            SearchValueFromReminderPage(searchValue);
        }

        //Create a TaskList from the reminders
        private TaskList CreateTaskList(IEnumerable<ReminderDb.Reminder> reminders)
        {
            var taskList = new TaskList();
            taskList.reminderListDBSub.Children.Clear();
            
            foreach (var reminder in reminders)
            {
                if (reminder != null)
                {
                    ReminderContent.Task task = new ReminderContent.Task();
                    task.AddInfo(reminder);
                    task.SingleSelectionUpdate += OnSingleSelectionUpdate;
                    task.MultiSelectionUpdate += OnMultiSelectionUpdate;

                    taskList.reminderListDBSub.Children.Add(task);
                }
            }
            return taskList;
        }

        //Update the UI with the TaskList
        private void UpdateUI()
        {
            if (CurrentTaskList == null) return;

            reminderListDB.Children.Clear();
            reminderListDB.Children.Add(CurrentTaskList);
        }

        private void UpdateSearchUI()
        {
            if (SearchedTaskList == null) return;

            reminderListDB.Children.Clear();
            reminderListDB.Children.Add(SearchedTaskList);
        }


        // Load data and update the UI
        public void LoadDataFromReminderPage()
        {
            if (_isUpdating) return;
            Debug.WriteLine("LoadData method invoked");

            _isUpdating = true;
            try
            {
                var reminders = FetchReminders();
                CurrentTaskList = CreateTaskList(reminders);
                UpdateUI();

                Debug.WriteLine("Data Loaded Succesfully");
            }
            finally
            {
                _isUpdating = false;
            }

        }

        //Load searched data and update the Ui
        public void SearchValueFromReminderPage(string searchValue)
        {
            if (_isUpdating) return;   

            _isUpdating = true ;
            try
            {
                Debug.WriteLine("Search triggered");

                SearchValue = searchValue;
                var reminders = FetchSearchValue(searchValue);
                SearchedTaskList = CreateTaskList(reminders);
                UpdateSearchUI();
            }
            finally
            { 
                _isUpdating = false; 
            }
        }


        // ReminderEdit event
        public void ReminderEditUpdate()
        {
            LoadDataFromReminderPage();
        }

        /*Single Selected Events*/

        //Update Reminder Editor using ReminderID on propertychange.
        private static void OnSelectedReminderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var reminderPage = d as ReminderPage;
            if (reminderPage != null)
            {
                var reminderEditor = (ReminderEditor)reminderPage.ReminderEditorContent;
                reminderEditor.CurrentUserId = reminderPage.UserId; 
                reminderEditor.CurrentFolderId = reminderPage.CurrentFolderId;
                reminderEditor.LoadData((int?)e.NewValue);
            }
        }

        //Get single selected reminder Id from Task control
        private void OnSingleSelectionUpdate(int reminderId)
        {
            SelectedReminderId = (int?)reminderId;
            OnReminderChanged(); //Trigger changes on the editor control 
            SelectedReminderIdChanged(reminderId);
        }

        //Event to invoke on Editor Control
        public virtual void OnReminderChanged()
        {
            if (_isUpdating) return;
            ReminderChanged?.Invoke(this, EventArgs.Empty);
        }

        /*Multi Selected Events*/

        //Update Reminder Editor using ReminderID List on propertychange.
        private static void OnSelectedReminderListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine($"On reminder list change event value:{e.NewValue}");
        }

        private void OnMultiSelectionUpdate(List<int?> list)
        {
            SelectedReminderIdList = (List<int?>)list;
            OnReminderListChanged();
            SelectedReminderListChanged(list);

            //testing
            Debug.WriteLine($"MultiSelection:");
            foreach (var item in list)
            {
                Debug.WriteLine($"Selected: {item}");
            }
        }

        //Event to invoke on Editor Control for multiple selection
        public virtual void OnReminderListChanged()
        {
            ReminderListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void LoadPersonalFolders()
        {
            PersonalFolder1 personalFolder1 = new PersonalFolder1();
            personalFolder1.UserId = UserId;
            
            PersonalFolder.Children.Add(personalFolder1);
        }
    }
}
