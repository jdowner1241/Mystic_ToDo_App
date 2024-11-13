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

        //Stackpanel version
        public void LoadDataFromReminderPage()
        {
            Debug.WriteLine("LoadData method invoked");

            var reminderList = DbContext.Reminders.ToList();
            TaskList taskList = new TaskList();
            taskList.reminderListDBSub.Children.Clear();
            reminderListDB.Children.Clear();

            foreach (var reminder in reminderList)
            {
                if (reminder != null) 
                {
                    ReminderContent.Task task = new ReminderContent.Task();
                    task.addInfo(reminder);
                    task.UpdateSelectedIdEvent = UpdateSelectedIdEvent;
                    task.SingleSelectionUpdate += OnSingleSelectionUpdate;
                    task.MultiSelectionUpdate += OnMultiSelectionUpdate;
                    

                    taskList.reminderListDBSub.Children.Add(task);
                }
            }
            reminderListDB.Children.Add(taskList);

            Debug.WriteLine("Data loaded successfully");
            OnReminderChanged();
        }

        private void UpdateSelectedIdEvent(int reminderId)
        {
            SelectedReminderId = reminderId;
            Debug.WriteLine("Selected reminder Id update on Reminder page");
            Debug.WriteLine(reminderId.ToString());

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

        protected virtual void OnReminderChanged()
        {
            ReminderChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnSingleSelectionUpdate(int reminderId)
        {
            UpdateSelectedIdEvent(reminderId);
            Debug.WriteLine($"SingleSection:");
            Debug.WriteLine($"Selected: {reminderId}");
        }

        private void OnMultiSelectionUpdate(List<int> list)
        {
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
