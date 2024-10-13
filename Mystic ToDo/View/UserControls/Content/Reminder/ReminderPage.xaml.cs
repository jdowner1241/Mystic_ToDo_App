using Mystic_ToDo.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Mystic_ToDo.View.UserControls.Content.Reminder
{
    /// <summary>
    /// Interaction logic for ReminderPage.xaml
    /// </summary>
    public partial class ReminderPage : UserControl
    {
        private readonly MysticToDo_DBEntities DbContext;
        

        public ReminderPage()
        {
            InitializeComponent();
            DbContext = new MysticToDo_DBEntities();
            LoadData();

        }

        private void LoadData()
        {
            var reminderList = DbContext.Reminders.ToList();
            reminderListDB.ItemsSource = reminderList;
        }

        private void clear()
        {

        }

        private void addReminder()
        {

        }

        private void removeReminder()
        {
            
        }
    }
}
