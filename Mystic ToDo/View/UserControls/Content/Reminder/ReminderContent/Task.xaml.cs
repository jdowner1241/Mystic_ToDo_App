using Mystic_ToDo.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
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

namespace Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent
{
    /// <summary>
    /// Interaction logic for Task.xaml
    /// </summary>
    public partial class Task : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _id;
        private bool _isComplete;
        private string _name;
        private string _description;
        private DateTime _date;
        private DateTime _time;
        private int _frequency; 

        public Task()
        {
            InitializeComponent();
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int ID
        {
            get { return _id; }
            set 
            {
                _id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        public bool _IsCompleted
        {
            get { return _isComplete; }
            set 
            { 
                _isComplete = value;
                OnPropertyChanged();
            }
        }
        
        public string _Name 
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string _Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public DateTime _Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        public DateTime _Time
        {
            get { return _time; }
            set 
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public int _Frequency
        {
            get { return _frequency; }
            set
            {
                _frequency = value;
                OnPropertyChanged();
            }
        }


        public void addInfo(ReminderDb.Reminder newReminder)
        {
            if (newReminder.IsComplete == false)
            {
                isCompleted.IsChecked = false;
            }
            else 
            {
                isCompleted.IsChecked = true;
            }

            name.Content = newReminder.Name;

            if (!string.IsNullOrEmpty(newReminder.Description))
            {
                description.Content = newReminder.Description;
            }

            if (newReminder.HasAlarms == true)
            {
                //_Date.Text = newReminder.Alarm.ToString();
                //_Time.Text = newReminder.Alarm.ToString();
                date.Text = "Date";
                time.Text = "Time";
            }
            else
            {
                date.Text = string.Empty;
            }

            if (newReminder.Periodic == true)
            {
                frequency.Text = "Periodic";
                //_Frequency.Text = newReminder.TimeFrameSelection.Name.ToString();
                //newReminder.TimeFrameSelection = DbContext.TimeFrames.Single(tf => tf.TimeFrameId == selectedTimeFrameId);
            }
            else
            {
               frequency.Text = string.Empty;
            }
        }

        public void addInfo1(ReminderDb.Reminder newReminder)
        {
            _id = newReminder.Id;
            _name = newReminder.Name;

            if (newReminder.IsComplete == false)
            {
                _isComplete = false;
            }
            else
            {
                _isComplete = true;
            }

            if (!string.IsNullOrEmpty(newReminder.Description))
            {
               _description = newReminder.Description;
            }
            else
            {
                _description = string.Empty;
            }

            if (newReminder.HasAlarms == true)
            {
                _date = DateTime.Now;
                _time = DateTime.Now;
                //date.Text = "Date";
                // time.Text = "Time";
            }
            else
            {
                date.Text = "Date";
                time.Text = "Time";
            }

            if (newReminder.Periodic == true)
            {
                _frequency = 1;
            }
            else
            {
                frequency.Text = string.Empty;
            }
        }


    }
}
