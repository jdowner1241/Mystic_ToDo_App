using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Interaction logic for SortItem.xaml
    /// </summary>
    public partial class SortItem : UserControl
    {
       public SortItem()
       {
           InitializeComponent();
           DataContext = this;

            // Populate the ComboBox with database columns
            PopulateColumnComboBox();
        }


        private void PopulateColumnComboBox()
        {
            using (var dbContext = new ReminderContext())
            {
                // Get the list of column names from the database table
                /*var columns = dbContext.Reminders.GetType().GetProperties()
                    .Select(p => p.Name)
                    .ToList();*/

                var columns = typeof(ReminderDb.Reminder).GetProperties().Select(p => p.Name).ToList();

                // Set the ItemsSource of the ComboBox
                columnComboBox.ItemsSource = columns; 
            } 
        }

        // properties for sorting
        public string SelectedColumn { get; set; }
        public string SortOrder { get; set; } 
    }

}
