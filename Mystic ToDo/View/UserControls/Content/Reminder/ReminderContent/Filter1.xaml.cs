using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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

namespace Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent
{
    /// <summary>
    /// Interaction logic for Filter1.xaml
    /// </summary>
    public partial class Filter1 : UserControl, INotifyPropertyChanged
    {
        public Filter1()
        {
            DataContext = this;
            InitializeComponent();
            searchAllFolders = false;

            AddFilterList();
        }

        private string searchValue;
        private bool searchAllFolders;
        private bool filterCompletedTrueOnly;
        private bool filterCompletedFalseOnly;

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<string, bool> SearchValueChanged;
        public event Action<bool, bool> FilterCompletedTrueOnlyValueChanged;
        public event Action<bool, bool> FilterCompletedFalseOnlyValueChanged;

        private ReminderPage reminderPage;

        public string SearchValue
        {
            get { return searchValue; }
            set
            {
                searchValue = value;
                OnPropertyChanged();
                SearchValueChanged?.Invoke(value, searchAllFolders);
            }
        }

        public bool SearchAllFolders
        {
            get { return searchAllFolders; }
            set
            {
                searchAllFolders = value;
                OnPropertyChanged();
                SearchValueChanged?.Invoke(SearchValue, value);
            }
        }

        public bool FilterCompletedTrueOnly
        {
            get { return filterCompletedTrueOnly; }
            set
            {
                filterCompletedTrueOnly = value;
                OnPropertyChanged();
                FilterCompletedTrueOnlyValueChanged?.Invoke(value, SearchAllFolders);
            }
        }

        public bool FilterCompletedFalseOnly
        {
            get { return filterCompletedFalseOnly; }
            set
            {
                filterCompletedFalseOnly = value;
                OnPropertyChanged();
                FilterCompletedFalseOnlyValueChanged?.Invoke(value, SearchAllFolders);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SubscribeToReminderPageEvents(ReminderPage reminderPage)
        {
            this.reminderPage = reminderPage;
            //SearchValueChanged -= reminderPage.SearchValueFromReminderPage;
            SearchValueChanged += reminderPage.SearchValueFromReminderPage;
            //SearchValueChanged += reminderPage.ReminderPageSearch;

            FilterCompletedTrueOnlyValueChanged += reminderPage.FilterCompletedTrueOnlyFromRP;
            FilterCompletedFalseOnlyValueChanged += reminderPage.FilterCompletedFalseOnlyFromRP;



            Debug.WriteLine("Subcribe to Filter1 Successful");
        }


        private void bClearSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            txtSearch.Focus();
        }


        private void bSearchAllToggle_Click(object sender, RoutedEventArgs e)
        {
            searchAllFolders = !searchAllFolders;

            if (searchAllFolders)
            {
                bSearchAllToggle.Content = "Search Within Selected Folder";
                bSearchAllToggle.Background = Brushes.LightBlue;
                SearchValueChanged?.Invoke(SearchValue, searchAllFolders);

                //SearchAllValueEnabledChanged?.Invoke(false);
            }
            else
            {
                bSearchAllToggle.Content = "Search All Folders";
                bSearchAllToggle.Background = Brushes.LightGray;
                //SearchAllValueEnabledChanged?.Invoke(true);
                SearchValueChanged?.Invoke(SearchValue, searchAllFolders);
            }
            // Notify the ReminderPage to update search behavior
        }

        private void AddFilterList()
        {
            FilterList.Children.Clear();

            // Create an instance of the FilterItems control
            var filterItemsControlCompleted = new FilterItems();

            // Update the header property
            filterItemsControlCompleted.Header = "Reminder Completed";

            // Add FilterItemsCbox items programmatically
            FilterItemsCbox itemTrue = new FilterItemsCbox { Text = "True" };
            FilterItemsCbox itemFalse = new FilterItemsCbox { Text = "False" };

            // Subscribe to events
            itemTrue.Checked += ItemTrue_Checked;
            itemTrue.Unchecked += ItemTrue_Unchecked;
            itemFalse.Checked += ItemFalse_Checked;
            itemFalse.Unchecked += ItemFalse_Unchecked;

            // Add items to the control
            filterItemsControlCompleted.AddFilterItem(itemTrue);
            filterItemsControlCompleted.AddFilterItem(itemFalse);

            // Add the filterItemsControl to a container in your main window or another parent control
            FilterList.Children.Add(filterItemsControlCompleted); // MainContainer should be your container control, like a StackPanel or Grid
        }

        // Event handlers for "True"
        private void ItemTrue_Checked(object sender, EventArgs e)
        {
            // Do something when "True" is checked
            //MessageBox.Show("True is checked");
            FilterCompletedTrueOnly = true;
        }

        private void ItemTrue_Unchecked(object sender, EventArgs e)
        {
            // Do something when "True" is unchecked
            //MessageBox.Show("True is unchecked");
            FilterCompletedTrueOnly = false;
        }

        // Event handlers for "False"
        private void ItemFalse_Checked(object sender, EventArgs e)
        {
            // Do something when "False" is checked
            //MessageBox.Show("False is checked");
            filterCompletedFalseOnly = true;
        }

        private void ItemFalse_Unchecked(object sender, EventArgs e)
        {
            // Do something when "False" is unchecked
            //MessageBox.Show("False is unchecked");
            filterCompletedFalseOnly = false;
        } 


    }
}
