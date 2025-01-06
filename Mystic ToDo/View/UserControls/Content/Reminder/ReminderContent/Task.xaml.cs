using Mystic_ToDo.Data;
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
using System.Windows.Media.Effects;
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

        private readonly ReminderContext DbContext;
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
        private bool _suppressUpdate;

        private bool singleSelected = false;
        private bool multiSelected = false;
        private List<int?> selectedTaskIds = new List<int?>();
        private List<Task> allTasks = new List<Task>();

        public event Action<List<int?>> MultiSelectionUpdate;
        public event Action<int> SingleSelectionUpdate;

        public Task()
        {
            InitializeComponent();
            DataContext = this;
            DbContext = new ReminderContext();

            this.MouseEnter += this.OnMouseEnter;
            this.MouseLeave += this.OnMouseLeave;
            this.MouseLeftButtonDown += this.OnMouseLeftButtonDown;

  
            TaskManager.RegisterTask(this);
     
        }

        ~Task()
        {
            TaskManager.UnregisterTask(this); 
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

                    if (!_suppressUpdate) // Only update status if the control is initialized
                    {
                        UpdateReminderStatus(); 
                    }
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
                if (_time != value)
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
                if (_frequencySelection != value)
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

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (!singleSelected && !multiSelected)
            {
                background.Background = Brushes.LightBlue;
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!singleSelected && !multiSelected)
            {
                background.Background = Brushes.LightGray;
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (DataContext == this)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    Debug.WriteLine("Multiselection");
                    Multiselection();
                    MultiSelectionUpdate?.Invoke(selectedTaskIds);
                }
                else
                {
                    Debug.WriteLine("Singleselection");
                    DeselectAllTasks();
                    Singleselection();
                    SingleSelectionUpdate?.Invoke(ID);
                }
            }
        }

        private void Multiselection()
        {

            if (multiSelected)
            {
                background.Background = Brushes.LightGray;
                selectedTaskIds.Remove(ID);
            }
            else
            {
                background.Background = Brushes.LightSkyBlue;
                if (!selectedTaskIds.Contains(ID))
                {
                    selectedTaskIds.Add(ID);
                }
            }
            multiSelected = !multiSelected;
        }

        private void Singleselection()
        {
            foreach (var task in allTasks)
            {
                if (task != this)
                {
                    task.Deselect();
                }
            }

            if (singleSelected)
            {
                background.Background = Brushes.LightGray;
                selectedTaskIds.Remove(ID);
            }
            else
            {
                background.Background = Brushes.LightSkyBlue;
                if (!selectedTaskIds.Contains(ID))
                {
                    selectedTaskIds.Add(ID);
                }
            }

            singleSelected = !singleSelected;
        }

        private void DeselectAllTasks()
        {
            foreach (var task in allTasks)
            {
                task.Deselect();
            }
            selectedTaskIds.Clear();
        }

        private void Deselect()
        {
            singleSelected = false;
            multiSelected = false;
            background.Background = Brushes.LightGray;

            selectedTaskIds.Remove(ID);
        }


        public void AddInfo(ReminderDb.Reminder newReminder)
        {
            // Suppress updates
            Task.SuppressUpdates();

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


            if (newReminder.SelectedUser != null && !string.IsNullOrEmpty(newReminder.SelectedUser.UserName))
            {
                UserName = newReminder.SelectedUser.UserName;
            }
            else
            {
                UserName = "Guest";
            }

            if (newReminder.SelectedFolder != null && !string.IsNullOrEmpty(newReminder.SelectedFolder.FolderName))
            {
                Folder = newReminder.SelectedFolder.FolderName;
            }
            else
            {
                Folder = "Default";
            }

            // Resume updates
            Task.ResumeUpdates();
        }

        private void IsCompleted_Checked(object sender, RoutedEventArgs e)
        {
            IsCompleted = true;
        }

        private void IsCompleted_Unchecked(object sender, RoutedEventArgs e)
        {
            IsCompleted = false;
        }

        private void UpdateReminderStatus()
        {
            var reminder = DbContext.Reminders.FirstOrDefault(r => r.Id == ID);

            if (reminder != null)
            {
                reminder.IsComplete = IsCompleted;
                DbContext.SaveChanges();
                //MessageBoxHelper.ShowMessageBox($"Reminder Completed Status Updated to {IsCompleted}");
                MessageBox.Show($"Reminder Completed Status Updated to {IsCompleted}");
            }
        }

        public static void SuppressUpdates() 
        { 
            foreach (var task in TaskManager.GetAllTasks()) 
            { 
                task._suppressUpdate = true;
            } 
        }

        public static void ResumeUpdates() 
        { 
            foreach (var task in TaskManager.GetAllTasks()) 
            { 
                task._suppressUpdate = false; 
            } 
        }
    }
}
