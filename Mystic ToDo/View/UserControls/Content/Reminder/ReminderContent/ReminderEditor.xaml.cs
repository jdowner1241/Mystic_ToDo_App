using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using static Mystic_ToDo.Database.ReminderDb;


namespace Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent
{
    /// <summary>
    /// Interaction logic for ReminderEditor.xaml
    /// </summary>
    public partial class ReminderEditor : UserControl
    {

        private ReminderContext DbContext;
        private ReminderDb.Reminder newReminder;
        private ReminderPage reminderPage;

        public event Action ReminderUpdate;

        public ReminderEditor()
        {
            DataContext = this;
            InitializeComponent();

            DbContext = new ReminderContext();
            newReminder = new ReminderDb.Reminder();

            SetCboxObj();
            dtpAlarm.Visibility = System.Windows.Visibility.Collapsed;
            cboxItems.Visibility = System.Windows.Visibility.Collapsed;

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
                MessageBox.Show("A reminder with this name already exists.");
                return;
            }


            DbContext.SaveReminder(addReminder);
            ReminderUpdate();
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
                        MessageBox.Show("Alarm required");
                        checkRepeat.IsChecked = false;
                    }
                    cboxItems.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }
    }
}
