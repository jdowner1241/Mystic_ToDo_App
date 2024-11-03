using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Mystic_ToDo.Database.ReminderDb;

namespace Mystic_ToDo.View.UserControls.Content.Reminder
{
    /// <summary>
    /// Interaction logic for ReminderPage.xaml
    /// </summary>
    public partial class ReminderPage : UserControl
    {
        private readonly ReminderContext DbContext;
        public ObservableCollection<int> CurrentReminderListIds { get; private set; }

        public ReminderPage()
        {
            InitializeComponent();
            DbContext = new ReminderContext();
            CurrentReminderListIds = new ObservableCollection<int>();
            LoadDataFromReminderPage();
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


        //Stackpanel version
        public void LoadDataFromReminderPage()
        {
            Debug.WriteLine("LoadData method invoked");

            var reminderList = DbContext.Reminders.ToList();
            TaskList taskList = new TaskList();
            

            foreach (var reminder in reminderList)
            {
                if (reminder != null) 
                {
                    ReminderContent.Task task = new ReminderContent.Task();
                    task.addInfo1(reminder);
                    taskList.reminderListDBSub.Children.Add(task);
                    
                }
            }
            reminderListDB.Children.Add(taskList);

            Debug.WriteLine("Data loaded successfully");
        }
       

    }
}
