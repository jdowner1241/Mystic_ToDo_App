using Mystic_ToDo.Data;
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
using TaskControl = Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent.Task;

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
            LoadDataFromReminderPage();

            Debug.Write($"\n\nReminderPage with UserID: {UserId} \n\n");

            InitializeControls();         

        }

        private readonly ReminderContext DbContext;
        private bool _isUpdating = false;
        private string searchValue;
        private bool _searchAllValueToggle;
        private bool filterCompletedTrueOnly;
        private bool filterCompletedFalseOnly;
        private string sortColumn;
        private string sortOrder;
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
                UpdateChildControls();
                LoadDataFromReminderPage();

                Debug.Write($"\n\nReminderPage updated with UserID: {UserId} \n\n");
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
                LoadDataFromReminderPage();
                //UpdateReminderEditorOnly();
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

        public bool SearchAllValueToggle
        {
            get => _searchAllValueToggle;
            set
            {
                _searchAllValueToggle = value;
            }
        }

        public bool FilterCompletedTrueOnly
        {
            get => filterCompletedTrueOnly;
            set
            {
                filterCompletedTrueOnly = value;
            }
        }

        public bool FilterCompletedFalseOnly
        {
            get => filterCompletedFalseOnly;
            set
            {
                filterCompletedFalseOnly = value;
            }
        }

        public string SortColumn
        {
            get => sortColumn;
            set
            {
                sortColumn = value;
            }
        }

        public string SortOrder
        {
            get => sortOrder;
            set
            {
                sortOrder = value;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// For Controls
        /// </summary>
        private void InitializeControls()
        {
            var personalFolder = (PersonalFolder1)FindName("PersonalFolder");
            if (personalFolder != null)
            {
                personalFolder.SelectedFolderIDUpdate -= OnFolderSelection;
                personalFolder.SelectedFolderIDUpdate += OnFolderSelection;
                personalFolder.LoadFolderList();
                
            }

            var reminderEditor = (ReminderEditor)FindName("ReminderEditorContent");
            if (reminderEditor != null)
            {
                reminderEditor.SubscribeToReminderPageEvents(this);
            }

            var filter = (Filter1)FindName("FilterContent");
            if (filter != null)
            {
                filter.SubscribeToReminderPageEvents(this);
            }
        }

        private void UpdateChildControls()
        {
            var personalFolder = (PersonalFolder1)FindName("PersonalFolder");
            if (personalFolder != null)
            {
                personalFolder.UserId = UserId; // Update the UserId for PersonalFolder
                personalFolder.SelectedFolderIDUpdate -= OnFolderSelection;
                personalFolder.SelectedFolderIDUpdate += OnFolderSelection;
                personalFolder.LoadFolderList(); // Reload folder list with updated UserId
            }

            UpdateReminderEditorOnly();
        }

        private void UpdateReminderEditorOnly()
        {
            var reminderEditor = (ReminderEditor)FindName("ReminderEditorContent");
            if (reminderEditor != null)
            {
                reminderEditor.CurrentUserId = UserId; // Update the UserId for ReminderEditor
                reminderEditor.CurrentFolderId = CurrentFolderId;
            }
        }

        /// <summary>
        /// Database 
        /// </summary>
        // Fetch reminders from the database for a specific user ID and folderId
        private List<ReminderDb.Reminder> FetchReminders()
        {
            if (UserId != 0 && CurrentFolderId != 0)
            {
                using (var context = new ReminderContext())
                {
                    var reminders = context.Reminders
                                            .Where(reminder => reminder.UserId == UserId && reminder.FolderId == CurrentFolderId)
                                            .ToList();
                    return reminders;
                }
            }
            else
            {
                Debug.WriteLine("User or Folder not set");
                return new List<ReminderDb.Reminder>(); // Return an empty list to handle the case where UserId is not set
            }
        }

        // Fetch reminders from the database for a specific user ID
        private List<ReminderDb.Reminder> FetchRemindersAllUsers()
        {
            if (UserId != 0 )
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
            var reminderList = FetchReminders(); // Retrieve reminders based on UserId  but folder specific 

            // Filter the retrieved reminders based on the search value
            if (searchValue != null)
            {
                var searchResults = reminderList
                                .Where(r => r.Name != null && r.Name.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                            r.Description != null && r.Description.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                                .ToList();

                return searchResults;
            }
            else
            {
                return reminderList;
            }
            
        }

        // Fetch all searched value from the database
        private List<ReminderDb.Reminder> FetchSearchValueAllUsers(string searchValue)
        {
            var reminderList = FetchRemindersAllUsers(); // Retrieve reminders based on UserId

            // Filter the retrieved reminders based on the search value
            if (searchValue != null)
            {
                var searchResults = reminderList
                                .Where(r => r.Name != null && r.Name.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                            r.Description != null && r.Description.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                                .ToList();

                return searchResults;
            }
            else
            {
                return reminderList;
            }
        }

        // Fetch Filtered OnlyTrue value from the database
        private List<ReminderDb.Reminder> FetchFilterTrueOnlyValue(bool filterCompletedTrueOnly)
        {
            var reminderList = FetchReminders(); // Retrieve reminders based on UserId but folder specific 

            // Filter the retrieved reminders based on the search value
            if (filterCompletedTrueOnly)
            {
                var filterResults = reminderList
                                .Where(r => r.IsComplete == true )
                                .ToList();

                return filterResults;
            }
            else
            {
                return reminderList;
            }

        }

        // Fetch Filtered OnlyTrue value from the database for all folders
        private List<ReminderDb.Reminder> FetchFilterTrueOnlyValueAllUsers(bool filterCompletedTrueOnly)
        {
            var reminderList = FetchRemindersAllUsers(); // Retrieve reminders based on UserId

            // Filter the retrieved reminders based on the IsCompleted Bool
            if (filterCompletedTrueOnly)
            {
                var filterResults = reminderList
                                    .Where(r => r.IsComplete == true)
                                    .ToList();

                return filterResults;
            }
            else
            {
                return reminderList;
            }
        }

        // Fetch Filtered OnlyFalse value from the database
        private List<ReminderDb.Reminder> FetchFilterFalseOnlyValue(bool filterCompletedFalseOnly)
        {
            var reminderList = FetchReminders(); // Retrieve reminders based on UserId but folder specific 

            // Filter the retrieved reminders based on the search value
            if (filterCompletedFalseOnly)
            {
                var filterResults = reminderList
                                .Where(r => r.IsComplete == false)
                                .ToList();

                return filterResults;
            }
            else
            {
                return reminderList;
            }

        }

        // Fetch Filtered OnlyFalse value from the database for all folders
        private List<ReminderDb.Reminder> FetchFilterFalseOnlyValueAllUsers(bool filterCompletedFalseOnly)
        {
            var reminderList = FetchRemindersAllUsers(); // Retrieve reminders based on UserId

            // Filter the retrieved reminders based on the IsCompleted Bool
            if (filterCompletedFalseOnly)
            {
                var filterResults = reminderList
                                    .Where(r => r.IsComplete == false)
                                    .ToList();

                return filterResults;
            }
            else
            {
                return reminderList;
            }
        }

        // Helper method to get property value using reflection 
        private object GetPropertyValue(object obj, string propertyName) 
        { 
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null); 
        }

        // Fetch Sort data from the database
        private List<ReminderDb.Reminder> FetchSortValue(string sortColumn, string sortOrder)
        {
            var reminderList = FetchReminders(); // Retrieve reminders based on UserId but folder specific 

            // Sort the retrieved reminders based on the SortColumn and order
            // Sort the retrieved reminders based on the SortColumn and order for all users
        
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortOrder))
            {
                // Apply sorting
                if (sortOrder.ToLower() == "Ascend")
                {
                    var SortResults = reminderList.OrderBy(r => GetPropertyValue(r, sortColumn)).ToList();
                    return SortResults;
                }
                else if (sortOrder.ToLower() == "Descend")
                {
                    var SortResults = reminderList.OrderByDescending(r => GetPropertyValue(r, sortColumn)).ToList();
                    return SortResults;
                }
            }
           return reminderList.ToList();
        }

        // Fetch Sort data from the database for all folders
        // Fetch Sort data from the database for all folders
        private List<ReminderDb.Reminder> FetchSortValueAllUsers(string sortColumn, string sortOrder)
        {
            var reminderList = FetchRemindersAllUsers(); // Retrieve reminders based on UserId

            // Sort the retrieved reminders based on the SortColumn and order for all users
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortOrder))
            {
                // Apply sorting
                if (sortOrder.ToLower() == "Ascend")
                {
                    var SortResults = reminderList.OrderBy(r => GetPropertyValue(r, sortColumn)).ToList();
                    return SortResults;
                }
                else if (sortOrder.ToLower() == "Descend")
                {
                    var SortResults = reminderList.OrderByDescending(r => GetPropertyValue(r, sortColumn)).ToList();
                    return SortResults;
                }
            }
            return reminderList.ToList();
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


        /// <summary>
        /// Events 
        /// </summary>
        // Event that gets and run the filterPage methods
        public void ReminderPageSearch(ReminderDb.Reminder reminder)
        {
            SearchValueFromReminderPage(SearchValue, SearchAllValueToggle);
            FilterCompletedTrueOnlyFromRP(FilterCompletedTrueOnly, SearchAllValueToggle);
            FilterCompletedFalseOnlyFromRP(FilterCompletedFalseOnly, SearchAllValueToggle);
            SortFromRP(SortColumn, SortOrder, SearchAllValueToggle);
        }


        //Load searched data and update the Ui
        public void SearchValueFromReminderPage(string searchValue, bool searchAllToggle)
        {
            if (_isUpdating) return;

            _isUpdating = true;
            try
            {
                SearchAllValueToggle = searchAllToggle;
                
                if (SearchAllValueToggle)
                {
                    var remindersAll = FetchSearchValueAllUsers(searchValue);
                    SearchedTaskList = CreateTaskList(remindersAll);// Search all folders
                    UpdateSearchUI();
                }
                else
                {
                    var reminders = FetchSearchValue(searchValue);
                    SearchedTaskList = CreateTaskList(reminders);// Search within the selected folder
                    UpdateSearchUI();
                }
            } 
            finally
            {
                _isUpdating = false;
            }
        }

        // Load filterControl data for Completed True Only and update the Ui 
        public void FilterCompletedTrueOnlyFromRP(bool filterTrueOnlyToggle, bool searchAllToggle)
        {
            if (_isUpdating) return;

            _isUpdating = true;
            try
            {
                SearchAllValueToggle = searchAllToggle;

                if (SearchAllValueToggle)
                {
                    var remindersAll = FetchFilterTrueOnlyValueAllUsers(filterTrueOnlyToggle);
                    SearchedTaskList = CreateTaskList(remindersAll);// Search all folders
                    UpdateSearchUI();
                }
                else
                {
                    var reminders = FetchFilterTrueOnlyValue(filterTrueOnlyToggle);
                    SearchedTaskList = CreateTaskList(reminders);// Search within the selected folder
                    UpdateSearchUI();
                }
            }
            finally
            {
                _isUpdating = false;
            }
        }

        // Load filterControl data for Completed False Only and update the Ui 
        public void FilterCompletedFalseOnlyFromRP(bool filterFalseOnlyToggle, bool searchAllToggle)
        {
            if (_isUpdating) return;

            _isUpdating = true;
            try
            {
                SearchAllValueToggle = searchAllToggle;

                if (SearchAllValueToggle)
                {
                    var remindersAll = FetchFilterFalseOnlyValueAllUsers(filterFalseOnlyToggle);
                    SearchedTaskList = CreateTaskList(remindersAll);// Search all folders
                    UpdateSearchUI();
                }
                else
                {
                    var reminders = FetchFilterFalseOnlyValue(filterFalseOnlyToggle);
                    SearchedTaskList = CreateTaskList(reminders);// Search within the selected folder
                    UpdateSearchUI();
                }
            }
            finally
            {
                _isUpdating = false;
            }
        }

        // Load SortControl data and update the Ui 
        public void SortFromRP(string sortColumn, string sortOrder, bool searchAllToggle)
        {
            if (_isUpdating) return;

            _isUpdating = true;
            try
            {
                SearchAllValueToggle = searchAllToggle;

                if (SearchAllValueToggle)
                {
                    var remindersAll = FetchSortValueAllUsers(sortColumn, sortOrder);
                    SearchedTaskList = CreateTaskList(remindersAll);// Search all folders
                    UpdateSearchUI();
                }
                else
                {
                    var reminders = FetchSortValue(sortColumn, sortOrder);
                    SearchedTaskList = CreateTaskList(reminders);// Search within the selected folder
                    UpdateSearchUI();
                }
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

        private void OnFolderSelection(int folderId) 
        {
            CurrentFolderId = folderId;
            UpdateReminderEditorOnly();
        }

    }
}
