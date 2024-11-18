using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using static Mystic_ToDo.Database.ReminderDb;

namespace Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent
{
    /// <summary>
    /// Interaction logic for TaskList.xaml
    /// </summary>
    public partial class TaskList : UserControl, INotifyPropertyChanged 
    {
        //private ObservableCollection<int> currectReminderListIds = new ObservableCollection<int>();
        private readonly ReminderContext DbContext;
        //private ReminderDb.Reminder newReminder;

        public TaskList()
        {
            DataContext = this;
            InitializeComponent();
            DbContext = new ReminderContext();

        }


        public event PropertyChangedEventHandler PropertyChanged;

    

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void AddReminderTolist(ReminderDb.Reminder newReminder)
        {
            Task reminderTask = new Task();
            if (newReminder == null)
            {
                reminderTask.AddInfo(newReminder);
            }

            //reminderListDBSub.Children.Add(reminderTask);
        }

        public void removeReminderTolist()
        {
            
        }
    }
}
