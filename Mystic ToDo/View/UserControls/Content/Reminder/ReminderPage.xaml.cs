using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
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
    public partial class ReminderPage : UserControl
    {
        private readonly ReminderContext DbContext;
        private bool _isUpdating = false;
        private TaskList CurrentTaskList { get; set; }

        public static readonly DependencyProperty SelectedReminderIdProperty = DependencyProperty.Register("SelectedReminderId", typeof(int?), typeof(ReminderPage), new PropertyMetadata(null, OnSelectedReminderChanged));
        public static readonly DependencyProperty SelectedReminderIdListProperty = DependencyProperty.Register("SelectedReminderIdList", typeof(List<int?>), typeof(ReminderPage), new PropertyMetadata(null, OnSelectedReminderListChanged));

        public event EventHandler ReminderChanged;
        public event Action<int?> SelectedReminderIdChanged;                                                                                
        public event EventHandler ReminderListChanged;
        public event Action<List<int?>> SelectedReminderListChanged;

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

        public ReminderPage()
        {
            InitializeComponent();
            DataContext = this;

            DbContext = new ReminderContext();
            LoadDataFromReminderPage();

            var reminderEditor = (ReminderEditor)FindName("ReminderEditorContent");
            if (reminderEditor != null)
            {
                reminderEditor.SubscribeToReminderPageEvents(this);
            }
        }

        //Fetch reminders from the database 
        private List<ReminderDb.Reminder> FetchReminders()
        {
            var renewDatabase = new ReminderContext().Reminders.ToList();
            return renewDatabase; 
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
    }
}
