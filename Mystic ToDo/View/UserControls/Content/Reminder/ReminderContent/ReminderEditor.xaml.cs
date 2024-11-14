﻿using Mystic_ToDo.Data;
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

        private ReminderContext DbContext;
        private ReminderDb.Reminder newReminder;
        private int CurrentID {  get; set; }
        private bool _editMode;

        private ReminderPage reminderPage;
        public event Action ReminderUpdate;
        public event Action ReminderEdited; 

        public event PropertyChangedEventHandler PropertyChanged;

        public ReminderEditor()
        {
            DataContext = this;
            InitializeComponent();

            DbContext = new ReminderContext();
            newReminder = new ReminderDb.Reminder();

            SetCboxObj();
            dtpAlarm.Visibility = System.Windows.Visibility.Collapsed;
            cboxItems.Visibility = System.Windows.Visibility.Collapsed;
            editMode = true;
            editMode = false;

        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SubscribeToReminderPageEvents(ReminderPage reminderPage)
        {
            this.reminderPage = reminderPage;
            reminderPage.ReminderChanged += ReminderPage_ReminderChanged;
            reminderPage.ReminderListChanged += ReminderPage_ReminderListChanged;
            Debug.WriteLine("RefreshEditor Event subcried correctly");
        }

        private void ReminderPage_ReminderChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Reminder changed event received in ReminderEditor");
            editMode = true;
            RefreshEditor();
            
        }

        private void ReminderPage_ReminderListChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("ReminderList changed event received in ReminderEditor");
            editMode = true;
            RefreshEditor();

        }

        private void RefreshEditor()
        {
            Debug.WriteLine("ReminderEditor is being refreshed");
            LoadData(CurrentID); 
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
            bAdd.Visibility= System.Windows.Visibility.Visible;
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
                dtpAlarm.getDateTime(); 
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
                        //newReminder.TimeFrameSelection = (ReminderDb.TimeFrameId)cboxItems.comboBox.SelectedIndex;

                        var selectedTimeFrameId = (ReminderDb.TimeFrameId)cboxItems.comboBox.SelectedIndex;
                        newReminder.TimeFrameSelection = DbContext.TimeFrames.Single(tf => tf.TimeFrameId == selectedTimeFrameId);
                    }
                    else
                    {
                        //newReminder.TimeFrameSelection = ReminderDb.TimeFrameId.NotSet;

                        var selectedTimeFrameId = ReminderDb.TimeFrameId.NotSet;
                        newReminder.TimeFrameSelection = DbContext.TimeFrames.Single(tf => tf.TimeFrameId == selectedTimeFrameId);
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

            newReminder.UserId = "YourUserIdHere";
            newReminder.Folder = "test";
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
                CurrentID = reminderId.Value;
                using ( var dbContext = new ReminderContext())
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
            catch (Exception ex) { }

        }

        private void bEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int orginalReminderID = CurrentID;
            var currentReminder = LoadFromForm();

            if (currentReminder != null) 
            {
                using (var dbContext = new ReminderContext())
                {
                    var existingReminder = dbContext.Reminders.SingleOrDefault(r => r.Id == orginalReminderID);

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
                        existingReminder.Folder = currentReminder.Folder;

                        dbContext.SaveChanges();
                        RefreshReminderList();
                        System.Windows.MessageBox.Show($"Reminder Updated!!! \nEdited Reminder: \nReminder ID:{orginalReminderID} \nReminder Name: {currentReminder.Name}");
                        reminderPage?.OnReminderChanged();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Reminder not found");
                    }
                }
            }
            ReminderEdited();
            ReminderUpdate();

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
