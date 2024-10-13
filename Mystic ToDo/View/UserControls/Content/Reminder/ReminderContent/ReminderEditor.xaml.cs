using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent
{
    /// <summary>
    /// Interaction logic for ReminderEditor.xaml
    /// </summary>
    public partial class ReminderEditor : UserControl
    {
        private ObservableCollection<string> alarmFreqencyList = new ObservableCollection<string> { "Not Set", "Daily", "Weekly", "Montly", "Yearly" };

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
            cboxItems.CboxItems = alarmFreqencyList;
        }

        private void bAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void bEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void bClear_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void bDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }


        private void checkAlarm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (checkAlarm != null && dtpAlarm != null)
            {
                if (e.PropertyName == nameof(checkAlarm.IsChecked))
                {
                    dtpAlarm.Visibility = checkAlarm.IsChecked == true ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                }
            }
                
        }


        private void checkRepeat_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
 
                if (e.PropertyName == nameof(checkRepeat.IsChecked))
                {
                    if (checkAlarm.IsChecked = true)
                    {
                        cboxItems.Visibility = checkRepeat.IsChecked ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;  
                    }
                    else
                    {
                        MessageBox.Show("Alarm required");
                    }
                }      
        }
    }
}
