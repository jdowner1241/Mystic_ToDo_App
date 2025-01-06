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
        }

        private string searchValue;
        private bool searchAllFolders;

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<string, bool> SearchValueChanged;

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
    }
}
