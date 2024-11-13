using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static readonly DependencyProperty SelectedReminderIdProperty = DependencyProperty.Register("SelectedReminderId", typeof(int?), typeof(ReminderPage), new PropertyMetadata(null, OnSelectedReminderChanged));

        public event EventHandler ReminderChanged;

        public int? SelectedReminderId
        {
            get => (int?)GetValue(SelectedReminderIdProperty);
            set => SetValue(SelectedReminderIdProperty, value);
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
            return DbContext.Reminders.ToList();
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
                    task.UpdateSelectedIdEvent = UpdateSelectedIdEvent;
                    task.SingleSelectionUpdate += OnSingleSelectionUpdate;
                    task.MultiSelectionUpdate += OnMultiSelectionUpdate;

                    taskList.reminderListDBSub.Children.Add(task);
                }
            }
            return taskList;
        } 
            
        //Update the UI with the TaskList
        private void UpdateUI(TaskList taskList)
        {
            reminderListDB.Children.Clear();
            reminderListDB.Children.Add(taskList);
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
                var taskList = CreateTaskList(reminders);
                UpdateUI(taskList);

                //RefreshTasks(reminders);

                Debug.WriteLine("Data Loaded Succesfully");
            }
            finally
            {
                _isUpdating = false;
            }
        }

        //Refresh logic for both new and existing reminders
        private void RefreshTasks(IEnumerable<ReminderDb.Reminder> reminders)
        {
            var existingTasks = reminderListDB.Children.OfType<ReminderContent.Task>().ToList();

            foreach (var reminder in reminders)
            {
                var task = existingTasks.FirstOrDefault(t => t.ID == reminder.Id);
                if (task != null)
                {
                    //Update existing task
                    task.AddInfo(reminder);
                }
                else
                {
                    //Add new task
                    var newTask = new ReminderContent.Task();
                    newTask.AddInfo(reminder);
                    newTask.UpdateSelectedIdEvent = UpdateSelectedIdEvent;
                    newTask.SingleSelectionUpdate += OnSingleSelectionUpdate;
                    newTask.MultiSelectionUpdate += OnMultiSelectionUpdate;

                    reminderListDB.Children.Add(newTask);
                }
            }
        }


        private void UpdateSelectedIdEvent(int reminderId)
        {
            SelectedReminderId = reminderId;
            OnReminderChanged();
        }

        private static void OnSelectedReminderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var reminderPage = d as ReminderPage;
            if (reminderPage != null)
            {
                var reminderEditor = (ReminderEditor)reminderPage.ReminderEditorContent;
                reminderEditor.LoadData((int?)e.NewValue);
            }
        }

        public virtual void OnReminderChanged()
        {
            if (_isUpdating) return;
            ReminderChanged?.Invoke(this, EventArgs.Empty);
        }

        public void OnReminderUpdated()
        {
            LoadDataFromReminderPage();
        }

        private void OnSingleSelectionUpdate(int reminderId)
        {
            UpdateSelectedIdEvent(reminderId);

            //testing
            Debug.WriteLine($"SingleSection:");
            Debug.WriteLine($"Selected: {reminderId}");
        }

        private void OnMultiSelectionUpdate(List<int> list)
        {

            //testing
            Debug.WriteLine($"MultiSelection:");
            foreach (var item in list) 
            {
                Debug.WriteLine($"Selected: {item}");
            }
        }


        //Grid version
        //public void LoadDataFromReminderPage()
        //{
        //    Debug.WriteLine("LoadData method invoked");

        //    if (DbContext.Reminders == null)
        //    {
        //        MessageBox.Show("Reminders DbSet is null");
        //    }

        //    var reminderList = DbContext.Reminders.ToList();

        //    reminderListDB.ItemsSource = reminderList;
        //    Debug.WriteLine("Data loaded successfully");
        //    reminderListDB.Items.Refresh();
        //}


    }
}
