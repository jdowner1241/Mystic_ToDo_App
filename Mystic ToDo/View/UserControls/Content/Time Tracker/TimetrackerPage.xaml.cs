using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
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
using System.Windows.Threading;

namespace Mystic_ToDo.View.UserControls.Content.Time_Tracker
{
    /// <summary>
    /// Interaction logic for TimetrackerPage.xaml
    /// </summary>
    public partial class TimetrackerPage : UserControl, INotifyPropertyChanged
    {
        public TimetrackerPage()
        {
            DataContext = this;
            InitializeComponent();

            InitializeClocks();
            InitializeStopwatch();
            InitializeTimesheet();
            InitializeAlarms();
        }

        private DispatcherTimer _timer;
        private DispatcherTimer _stopwatchTimer;
        private DispatcherTimer _countdownTimer;
        private TimeSpan _stopwatchTime;
        private TimeSpan _timerRemaining;
        private ObservableCollection<TimeSheetEntry> _timesheetEntries;
        private ObservableCollection<Alarm> _alarms;

        private int _currentUserId;

        public event PropertyChangedEventHandler PropertyChanged;

        public int CurrentUserId
        {
            get => _currentUserId; set
            {
                _currentUserId = value;
                OnPropertyChanged(nameof(CurrentUserId));
                // Perform any action needed when CurrentUserId changes, e.g., loading data for the user
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void InitializeClocks()
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += ClockTimer_Tick;
            _timer.Start();
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            ClockText.Text = DateTime.Now.ToString("hh:mm:ss tt");
            CheckAlarms();
        }

        private void InitializeStopwatch()
        {
            _stopwatchTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _stopwatchTimer.Tick += StopwatchTimer_Tick;
            _stopwatchTime = TimeSpan.Zero;
        }

        private void StopwatchTimer_Tick(object sender, EventArgs e)
        {
            _stopwatchTime = _stopwatchTime.Add(TimeSpan.FromMilliseconds(100));
            StopwatchText.Text = _stopwatchTime.ToString(@"hh\:mm\:ss\.ff");
        }

        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_stopwatchTimer.IsEnabled)
            {
                _stopwatchTimer.Stop();
                StartStopButton.Content = "Start";
            }
            else
            {
                _stopwatchTimer.Start();
                StartStopButton.Content = "Stop";
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _stopwatchTimer.Stop();
            _stopwatchTime = TimeSpan.Zero;
            StopwatchText.Text = "00:00:00.00";
            StartStopButton.Content = "Start";
        }

        private void InitializeAlarms()
        {
            _alarms = new ObservableCollection<Alarm>();
            AlarmListBox.ItemsSource = _alarms;
        }

        private void CheckAlarms()
        {
            foreach (var alarm in _alarms.ToList())
            {
                if (alarm.AlarmTime.Hours != DateTime.Now.Hour ||
                    alarm.AlarmTime.Minutes != DateTime.Now.Minute ||
                    DateTime.Now.Second != 0 ||
                    alarm.DayOfWeek != DateTime.Now.DayOfWeek.ToString())
                {
                    continue;
                }
                MessageBox.Show($"Alarm: {alarm.DayOfWeek} at {alarm.AlarmTime:hh\\:mm}");
                _alarms.Remove(alarm);
            }
        }

        private void AddAlarmButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimeSpan.TryParse(AlarmTimeTextBox.Text, out var alarmTime) && AlarmDayComboBox.SelectedItem != null)
            {
                var day = ((ComboBoxItem)AlarmDayComboBox.SelectedItem).Content.ToString();
                _alarms.Add(new Alarm { AlarmTime = alarmTime, DayOfWeek = day }); // Use AlarmTime property
                AlarmTimeTextBox.Clear();
                AlarmDayComboBox.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Please enter a valid time and select a day.");
            }
        }

        private void DeleteAlarmButton_Click(object sender, RoutedEventArgs e)
        {
            if (AlarmListBox.SelectedItem is Alarm selectedAlarm)
            {
                _alarms.Remove(selectedAlarm);
            }
        }

        private void InitializeTimesheet()
        {
            _timesheetEntries = new ObservableCollection<TimeSheetEntry>();
            TimesheetDataGrid.ItemsSource = _timesheetEntries;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskTextBox.Text))
            {
                var entry = new TimeSheetEntry
                {
                    Task = TaskTextBox.Text,
                    StartTime = DateTime.Now.ToString("hh:mm tt"),
                    EndTime = DateTime.Now.AddMinutes(30).ToString("hh:mm tt"),
                    TotalTime = "30 min"
                };
                _timesheetEntries.Add(entry);
                TaskTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a task.");
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimesheetDataGrid.SelectedItem is TimeSheetEntry selectedEntry)
            {
                _timesheetEntries.Remove(selectedEntry);
            }
        }

        private void StartTimerButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimeSpan.TryParse(TimerDurationTextBox.Text, out var duration))
            {
                _timerRemaining = duration;
                TimerText.Text = _timerRemaining.ToString(@"hh\:mm\:ss");

                _countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                _countdownTimer.Tick += CountdownTimer_Tick;
                _countdownTimer.Start();
            }
            else
            {
                MessageBox.Show("Please enter a valid duration (hh:mm:ss).");
            }
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (_timerRemaining > TimeSpan.Zero)
            {
                _timerRemaining = _timerRemaining.Subtract(TimeSpan.FromSeconds(1));
                TimerText.Text = _timerRemaining.ToString(@"hh\:mm\:ss");
            }
            else
            {
                _countdownTimer.Stop();
                MessageBox.Show("Timer finished!");
                TimerText.Text = "00:00:00";
            }
        }

        private void StopTimerButton_Click(object sender, RoutedEventArgs e)
        {
            _countdownTimer?.Stop();
            TimerText.Text = "00:00:00";
        }

        private void EditTimerButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Timer editing is not implemented yet.");
        }

        private void ClearTimerButton_Click(object sender, RoutedEventArgs e)
        {
            TimerText.Text = "00:00:00";
            TimerDurationTextBox.Clear();
            _countdownTimer?.Stop();
        }

        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class Alarm
    {
        public TimeSpan AlarmTime { get; set; }
        public string DayOfWeek { get; set; }
    }


    public class TimeSheetEntry
    {
        public string Task { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TotalTime { get; set; }
    }

}
