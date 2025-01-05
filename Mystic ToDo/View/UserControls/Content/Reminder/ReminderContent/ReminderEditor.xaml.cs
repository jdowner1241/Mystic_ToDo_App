using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using static Mystic_ToDo.Database.ReminderDb;


namespace Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent
{
    /// <summary>
    /// Interaction logic for ReminderEditor.xaml
    /// </summary>
    public partial class ReminderEditor : UserControl, INotifyPropertyChanged
    {

        public ReminderEditor()
        {
            InitializeComponent();
            DataContext = this;

            DbContext = new ReminderContext();
            newReminder = new ReminderDb.Reminder();

            SetCboxObj();
            dtpAlarm.Visibility = System.Windows.Visibility.Collapsed;
            cboxItems.Visibility = System.Windows.Visibility.Collapsed;
            editMode = true;
            editMode = false;

            Debug.Write($"\n\nReminderEditor with UserID: {CurrentUserId} \n\n");
        }

        private ReminderContext DbContext;
        private ReminderDb.Reminder newReminder;
        private int CurrentId { get; set; }
        private bool singleSelected;
        private List<int> CurrentIdList { get; set; }
        private int _currentUserId;
        private int _currentFolderId;
        private bool multiSelected;
        private bool _editMode;

        private ReminderPage reminderPage;
        public event Action ReminderUpdate;
        public event PropertyChangedEventHandler PropertyChanged;

        public int CurrentUserId
        {
            get { return _currentUserId; } 
            set
            {
                _currentUserId = value;
                OnPropertyChanged();
                Debug.Write($"\n\nReminderEditor set UserID: {CurrentUserId} \n\n");
            } 
        }

        public int CurrentFolderId 
        { 
            get { return _currentFolderId; }
            set 
            { 
                _currentFolderId = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void SubscribeToReminderPageEvents(ReminderPage reminderPage)
        {
            this.reminderPage = reminderPage;
            reminderPage.ReminderChanged += ReminderPage_ReminderChanged;
            reminderPage.SelectedReminderIdChanged += OnSelectedReminderIdChanged;
            reminderPage.ReminderListChanged += ReminderPage_ReminderListChanged;
            reminderPage.SelectedReminderListChanged += OnSelectedReminderIdListChanged;

            Debug.WriteLine("RefreshEditor Event subcried correctly");
        }

        private void OnSelectedReminderIdChanged(int? selectedReminderId)
        {
            CurrentId = selectedReminderId.Value;
        }

        private void OnSelectedReminderIdListChanged(List<int?> list)
        {
            CurrentIdList = list.Where(x => x.HasValue).Select(x => x.Value).ToList();
        }


        private void ReminderPage_ReminderChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Reminder changed event received in ReminderEditor");
            editMode = true;
            singleSelected = true;
            multiSelected = false;
            RefreshEditor();

        }

        private void ReminderPage_ReminderListChanged(object sender, 
            EventArgs e)
        {
            Debug.WriteLine("ReminderList changed event received in " +
                "ReminderEditor");
            editMode = true;
            multiSelected = true;
            singleSelected = false;
            //RefreshEditor();

        }

        private void RefreshEditor()
        {
            Debug.WriteLine("ReminderEditor is being refreshed");
            LoadData(CurrentId);
        }

        private void RefreshReminderList()
        {
            // ReminderUpdate();
            ReminderUpdate?.Invoke();
        }

        public bool editMode
        {
            get => _editMode;
            set
            {
                if (_editMode != value)
                {
                    _editMode = value;
                    OnPropertyChanged();

                    if (_editMode)
                    {
                        EnableEditMode();
                    }
                    else
                    {
                        DisableEditMode();
                    }
                }
            }
        }


        private void EnableEditMode()
        {
            bAddNew.Visibility = System.Windows.Visibility.Visible;
            bEdit.Visibility = System.Windows.Visibility.Visible;
            bAdd.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void DisableEditMode()
        {
            bAdd.Visibility = System.Windows.Visibility.Visible;
            bAddNew.Visibility = System.Windows.Visibility.Collapsed;
            bEdit.Visibility = System.Windows.Visibility.Collapsed;
        }


        private ReminderDb.Reminder LoadFromForm()
        {

            //Name
            if (!string.IsNullOrEmpty(txtboxName.txtBox.Text))
            {
                newReminder.Name = txtboxName.txtBox.Text;
            }

            //Description
            if (!string.IsNullOrEmpty(txtboxDescription.txtBox.Text))
            {
                newReminder.Description = txtboxDescription.txtBox.Text;
            }
            else
            {
                newReminder.Description = string.Empty;
            }

            //HasAlarm
            if (checkAlarm.IsChecked == true)
            {
                newReminder.HasAlarms = true;

                //Alarm
                //dtpAlarm.getDateTime();
                if (dtpAlarm.DateWithTime != null)
                {
                    newReminder.Alarm = dtpAlarm.DateWithTime;
                }
                else
                {
                    newReminder.Alarm = null;
                }

                //Periodic
                if (checkRepeat.IsChecked == true)
                {
                    newReminder.Periodic = true;

                    //TimeFrameSelection
                    if (cboxItems.comboBox.SelectedIndex != -1)
                    {
                        newReminder.TimeFrameId = (TimeFrameId)cboxItems.comboBox.SelectedIndex;
                        newReminder.TimeFrameSelection = DbContext.TimeFrames.SingleOrDefault(tf => tf.TimeFrameId == newReminder.TimeFrameId);
                        
                       
                    }
                    else
                    {
                        newReminder.TimeFrameId = TimeFrameId.NotSet;
                        newReminder.TimeFrameSelection = DbContext.TimeFrames.SingleOrDefault(tf => tf.TimeFrameId == newReminder.TimeFrameId);
                    }
                }
                else
                {
                    newReminder.Periodic = false;
                }
            }
            else
            {
                newReminder.HasAlarms = false;
            }

            if (CurrentUserId != 1)
            {
                newReminder.UserId = CurrentUserId;
                newReminder.SelectedUser = DbContext.Users.SingleOrDefault(f => f.UserId == newReminder.UserId);
            }
            else
            {
                newReminder.UserId = 1;
                newReminder.SelectedUser = DbContext.Users.SingleOrDefault(f => f.UserId == newReminder.UserId);
            }

            if (newReminder.SelectedUser == null) { throw new InvalidOperationException("The specified UserId was not found."); }

            if (CurrentFolderId != 1)
            {
                newReminder.FolderId = CurrentFolderId;
                newReminder.SelectedFolder = DbContext.Folders.SingleOrDefault(f => f.FolderId == newReminder.FolderId);
            }
            else 
            {
                newReminder.FolderId = 1;
                newReminder.SelectedFolder = DbContext.Folders.SingleOrDefault(f => f.FolderId == newReminder.FolderId);
            }

            if (newReminder.SelectedFolder == null) { throw new InvalidOperationException("The specified FolderId was not found."); }

            return newReminder;
        }


        private void SaveToDatabase(ReminderDb.Reminder addReminder)
        {
            var existingReminder = DbContext.Reminders.FirstOrDefault(r => r.Name == addReminder.Name);

            if (existingReminder != null)
            {
                // Handle case where a reminder with the same name already exists
                System.Windows.MessageBox.Show("A reminder with this name already exists.");
                return;
            }


            DbContext.SaveReminder(addReminder);
            RefreshReminderList();
        }

        public void LoadData(int? reminderId)
        {
            if (reminderId.HasValue)
            {
                CurrentId = reminderId.Value;
                using (var dbContext = new ReminderContext())
                {
                    var reminderDetail = dbContext.Reminders.FirstOrDefault(r => r.Id == reminderId.Value);
                    if (reminderDetail != null)
                    {
                        LoadToForm(reminderDetail);
                    }
                }
            }
        }

        private void LoadToForm(ReminderDb.Reminder reminder)
        {
            // Name
            txtboxName.txtBox.Text = reminder.Name;
            reminder.UserId = CurrentUserId;
            reminder.SelectedUser = DbContext.Users.SingleOrDefault(f => f.UserId == reminder.UserId);
            reminder.FolderId = CurrentFolderId;
            reminder.SelectedFolder = DbContext.Folders.SingleOrDefault(f => f.FolderId == reminder.FolderId);

            // Description
            txtboxDescription.txtBox.Text = reminder.Description ?? string.Empty;

            // HasAlarm
            checkAlarm.IsChecked = reminder.HasAlarms;

            if (reminder.HasAlarms)
            {
                dtpAlarm.Visibility = System.Windows.Visibility.Visible;
                dtpAlarm.DateWithTime = reminder.Alarm;


                checkRepeat.IsChecked = reminder.Periodic;
                if (reminder.Periodic)
                {
                    cboxItems.Visibility = System.Windows.Visibility.Visible;
                    cboxItems.comboBox.SelectedIndex = (int)reminder.TimeFrameId;
                }
                else
                {
                    cboxItems.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else
            {
                dtpAlarm.Visibility = System.Windows.Visibility.Collapsed;
                dtpAlarm.DateWithTime = null;
                checkRepeat.IsChecked = false;
                cboxItems.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public void SetCboxObj()
        {
            var timeFrames = Enum.GetValues(typeof(ReminderDb.TimeFrameId))
                                 .Cast<ReminderDb.TimeFrameId>()
                                 .Select(e => e.ToString())
                                 .ToList();

            cboxItems.CboxItems.Clear();

            foreach (var timeFrame in timeFrames)
            {
                cboxItems.CboxItems.Add(timeFrame);
            }
        }

        private void bAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var newReminder = LoadFromForm();
                if (newReminder != null)
                {
                    SaveToDatabase(newReminder);
                }
            }
            catch (Exception ex) 
            {
                System.Windows.MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }

        private void bEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int orginalReminderID = CurrentId;
            var currentReminder = LoadFromForm();

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(
                " Do you want to override the current Reminder?",
                "Override",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
                );

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (currentReminder != null)
                {
                    using (var dbContext = new ReminderContext())
                    {
                        var existingReminder = dbContext.Reminders.SingleOrDefault(r => r.Id == orginalReminderID);
                        var beforechange = existingReminder;

                        System.Windows.MessageBox.Show($"Reminder Updated!!! \nReminder ID: {orginalReminderID} " +
                            $"\n\nBefore: \nReminder Name: {beforechange.Name}" +
                            $"\n\nAfter: \nReminder Name: {currentReminder.Name}");

                        if (existingReminder != null)
                        {
                            existingReminder.Name = currentReminder.Name;
                            existingReminder.Description = currentReminder.Description;
                            existingReminder.IsComplete = currentReminder.IsComplete;
                            existingReminder.HasAlarms = currentReminder.HasAlarms;
                            existingReminder.Alarm = currentReminder.Alarm;
                            existingReminder.Periodic = currentReminder.Periodic;
                            existingReminder.TimeFrameId = currentReminder.TimeFrameId;
                            existingReminder.UserId = currentReminder.UserId;
                            existingReminder.SelectedUser = currentReminder.SelectedUser;
                            existingReminder.FolderId = currentReminder.FolderId;
                            existingReminder.SelectedFolder = currentReminder.SelectedFolder;

                            dbContext.SaveChanges();
                            RefreshReminderList();
                            
                            reminderPage?.OnReminderChanged();
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Reminder not found");
                        }
                    }
                }
                ReminderUpdate();
            }
        }

        private void bClear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (txtboxName.txtBox.Text != string.Empty)
            {
                txtboxName.txtBox.Clear();
            }

            if (txtboxDescription.txtBox.Text != string.Empty)
            {
                txtboxDescription.txtBox.Clear();
            }

            if (checkAlarm.IsChecked == true)
            {
                if (checkRepeat.IsChecked == true)
                {
                    cboxItems.comboBox.SelectedIndex = 0;
                }
                dtpAlarm.datePicker.ClearValue(DatePicker.SelectedDateProperty);
                checkRepeat.IsChecked = false;
                checkAlarm.IsChecked = false;

            }
            editMode = false;
        }

        private void bDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var currentReminder = LoadFromForm();

            MessageBoxResult result = System.Windows.MessageBox.Show(
                $" Do you want to Delete this reminder? \n Reminder Name: {currentReminder.Name}",
                "Delete Reminder !!!",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.OK) 
            {
                if (singleSelected)
                {
                    DeleteSingleReminder();
                }

                if (multiSelected)
                {
                    DeleteMultipleReminders();
                }

                ReminderUpdate();
            }
        }

        private void DeleteSingleReminder()
        {
            using (var dbContext = new ReminderContext())
            {
                var existingReminder = dbContext.Reminders.SingleOrDefault(r => r.Id == CurrentId);

                if (existingReminder != null)
                {
                    System.Windows.MessageBox.Show($"Reminder Removed!!! \n\nReminder ID: {existingReminder.Id} \nReminder Name: {existingReminder.Name}");
                    dbContext.Reminders.Remove(existingReminder);

                    dbContext.SaveChanges();
                    RefreshReminderList();

                    reminderPage?.OnReminderChanged();
                }
                else
                {
                    System.Windows.MessageBox.Show("Reminder not found");
                }
            }
            singleSelected = false;
        }

        private void DeleteMultipleReminders()
        {
            string message = "Reminder Removed:";

            using (var dbContext = new ReminderContext())
            {
                foreach (var CurrentId in CurrentIdList)
                {
                    var existingReminder = dbContext.Reminders.SingleOrDefault(r => r.Id == CurrentId);

                    if (existingReminder != null)
                    {
                        message += $"\n\nReminder ID: {existingReminder.Id} \nReminder Name: {existingReminder.Name}";

                        dbContext.Reminders.Remove(existingReminder);

                        dbContext.SaveChanges();
                        RefreshReminderList();

                        reminderPage?.OnReminderChanged();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Reminder not found");
                    }

                }
            }
            System.Windows.MessageBox.Show(message);
            multiSelected = false;
        }


        private void checkAlarm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(checkAlarm.IsChecked))
            {
                if (checkAlarm.IsChecked == true)
                {
                    dtpAlarm.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    dtpAlarm.Visibility = System.Windows.Visibility.Collapsed;
                    if (checkRepeat.IsChecked == true)
                    {
                        checkRepeat.IsChecked = false; // This will trigger the checkRepeat_PropertyChanged
                    }
                }
            }
        }

        private void checkRepeat_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(checkRepeat.IsChecked))
            {
                if (checkAlarm.IsChecked == true)
                {
                    if (checkRepeat.IsChecked == true)
                    {
                        cboxItems.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        cboxItems.Visibility = System.Windows.Visibility.Collapsed;
                        checkAlarm.IsChecked = false; // This will trigger the checkAlarm_PropertyChanged
                    }
                }
                else
                {
                    if (checkRepeat.IsChecked == true)
                    {
                        System.Windows.MessageBox.Show("Alarm required");
                        checkRepeat.IsChecked = false;
                    }
                    cboxItems.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }
    }
}
