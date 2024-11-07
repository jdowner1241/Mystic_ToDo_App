using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using Mystic_ToDo.View.UserControls.Content.Reminder;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
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

namespace Mystic_ToDo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ReminderContext ReminderList;
        private ReminderPage _reminderPage;
        private int? _selectedReminderId;

        public int? SelectedReminderId
        {
            get => _selectedReminderId;
            set
            {
                _selectedReminderId = value;
                OnPropertyChanged(nameof(SelectedReminderId));
            }
        }

        public MainWindow()
        {
            InitializeComponent();


            ReminderList = new ReminderContext();
            _reminderPage = new ReminderPage();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateSelectedReminderIdEvent(int reminderId)
        {
            SelectedReminderId = reminderId;
        }
    }
}
