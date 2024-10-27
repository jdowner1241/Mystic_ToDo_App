using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent;
using System;
using System.Diagnostics;
using System.Linq;
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

        public ReminderPage()
        {
            InitializeComponent();
            DbContext = new ReminderContext();
            LoadDataFromReminderPage();
        }

        public void LoadDataFromReminderPage()
        {
            Debug.WriteLine("LoadData method invoked");

            if (DbContext.Reminders == null)
            {
                MessageBox.Show("Reminders DbSet is null");
            }

            var reminderList = DbContext.Reminders.ToList();

            reminderListDB.ItemsSource = reminderList;
            Debug.WriteLine("Data loaded successfully");
            reminderListDB.Items.Refresh();
        }

    }
}
