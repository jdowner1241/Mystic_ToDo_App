using Mystic_ToDo.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
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
using System.Windows.Shell;

namespace Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent
{
    /// <summary>
    /// Interaction logic for Task.xaml
    /// </summary>
    public partial class Task : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _id;
        private bool _isComplete;
        private string _name;
        private string _description;
        private DateTime _dateWithTime;
        private string _date;
        private string _time;
        private int _frequencySelection; 
        private string _frequency;
        private string _folder;
        private string _userName;


        public Task()
        {
            InitializeComponent();
            DataContext = this; 
            this.MouseEnter += this.OnMouseEnter;
            this.MouseLeave += this.OnMouseLeave;
            this.MouseLeftButtonDown += this.OnMouseLeftButtonDown;
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static readonly DependencyProperty UpdateSelectedIdEventProperty = DependencyProperty.Register("UpdateSelectedIDEvent", typeof(Action<int>), typeof(Task), new PropertyMetadata(null));


        public Action<int> UpdateSelectedIdEvent
        {
            get => (Action<int>)GetValue(UpdateSelectedIdEventProperty);
            set => SetValue(UpdateSelectedIdEventProperty, value);
        }

        public int ID
        {
            get => _id;
            set 
            {
                if (_id != value) 
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsCompleted
        {
            get => _isComplete;
            set 
            {
                if (_isComplete != value) 
                {
                    _isComplete = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public string ReminderName
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DateWithTime
        {
            get => _dateWithTime;
            set
            {
                if (_dateWithTime != value)
                {
                    _dateWithTime = value;
                    Date = _dateWithTime.ToString("D", CultureInfo.InvariantCulture);
                    Time = _dateWithTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                    OnPropertyChanged();
                }
            }
        }

        public string Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value; 
                    OnPropertyChanged();
                }
            }
        }

        public string Time
        {
            get => _time;
            set 
            {
                if(_time != value)
                {
                    _time = value;
                    OnPropertyChanged();
                }
            }
        }

        public int FrequencySelection
        {
            get => _frequencySelection;
            set
            {
                if ( _frequencySelection != value)
                {
                    _frequencySelection = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Frequency
        {
            get => _frequency; 
            set
            {
                if (_frequency != value)
                {
                    _frequency = value; 
                    OnPropertyChanged();
                }
            }
        }

        public string Folder
        {
            get => _folder;
            set
            {
                if (_folder != value)
                {
                    _folder = value;
                    OnPropertyChanged();
                }
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged();
                }
            }
        }

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty BackgroudProperty = 
            DependencyProperty.Register("Background", typeof(Brush), typeof(Task), new PropertyMetadata(Brushes.LightGray));

        private void OnMouseEnter (object sender, MouseEventArgs e)
        {
            backgroud.Background = Brushes.LightBlue;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            backgroud.Background = Brushes.LightGray;
        }

        private void OnMouseLeftButtonDown(object sender, MouseEventArgs e) 
        {
            if(DataContext == this)
            {
                UpdateSelectedIdEvent?.Invoke(ID);
                Debug.WriteLine("Selected reminder Id and event Invoked");
            }
        }

        public void addInfo(ReminderDb.Reminder newReminder)
        {
            ID = newReminder.Id;
            ReminderName = newReminder.Name;

            if (newReminder.IsComplete == false)
            {
                IsCompleted = false;
            }
            else
            {
                IsCompleted = true;
            }

            if (!string.IsNullOrEmpty(newReminder.Description))
            {
               Description = newReminder.Description;
            }
            else
            {
                Description = string.Empty;
            }

            if (newReminder.HasAlarms == true)
            {
                DateWithTime = (DateTime)newReminder.Alarm;
            }
            else
            {
                Date = "not set";
                Time = "not set";
            }

            if (newReminder.Periodic == true)
            {
                if (newReminder.TimeFrameId != 0)
                {
                    Frequency = newReminder.TimeFrameId.ToString();
                    FrequencySelection = (int)newReminder.TimeFrameId;
                }
            }
            else
            {
                Frequency = "Not set";
                FrequencySelection = 0;
            }

            if (!string.IsNullOrEmpty(newReminder.UserId))
            {
                UserName = newReminder.UserId;
            }else
            {
                UserName = "Anonymous";
            }

            if (!string.IsNullOrEmpty(newReminder.Folder))
            {
                Folder = newReminder.Folder;
            }else
            {
                Folder = "Folder Not set";
            }
        }


    }
}
