using Mystic_ToDo.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
    /// Interaction logic for Filter.xaml
    /// </summary>
    public partial class Filter : UserControl
    {
        public Filter()
        {
            InitializeComponent();
            loadData();
        }

        private void loadData()
        {
            //var reminder = fetchReminders();
            //var reminders = new ObservableCollection<ReminderDb.Reminder>(reminder);

        }


        private void FilteringTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            String selectedFiltering = NormalSearchBox.Text;
              if (String.IsNullOrEmpty(selectedFiltering))
            {
                
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
           
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
