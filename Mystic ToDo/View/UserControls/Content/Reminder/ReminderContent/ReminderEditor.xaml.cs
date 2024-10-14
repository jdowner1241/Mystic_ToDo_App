using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent
{
    /// <summary>
    /// Interaction logic for ReminderEditor.xaml
    /// </summary>
    public partial class ReminderEditor : UserControl
    {

        private enum alarmFrequencyList
        {
            NotSet = 0,
            Daily = 1,
            Weekly = 2,
            Monthly = 3,
            Yearly = 4
        }

        public ReminderEditor()
        {
            InitializeComponent();
            DataContext = this;
            setCboxObj();
            dtpAlarm.Visibility = System.Windows.Visibility.Collapsed;
            cboxItems.Visibility = System.Windows.Visibility.Collapsed;
            //checkAlarm.IsEnabled = false;
            //checkRepeat.IsEnabled = false;
            //dtpAlarm.
        }

        public void setCboxObj()
        {
            cboxItems.CboxItems.Clear(); // Clear existing items if any
            foreach (var value in Enum.GetValues(typeof(alarmFrequencyList)))
            {
                cboxItems.CboxItems.Add(value.ToString());
            }
        }

        private void bAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {

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
