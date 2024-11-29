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
        }

        private string searchValue;

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<string> SearchValueChanged;

        private ReminderPage reminderPage;

        public string SearchValue
        {
            get { return searchValue; }
            set 
            { 
                searchValue = value;
                OnPropertyChanged();
                SearchValueChanged?.Invoke(value);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SubscribeToReminderPageEvents(ReminderPage reminderPage)
        {
            this.reminderPage = reminderPage;
            SearchValueChanged += reminderPage.SearchValueFromReminderPage;
            //SearchValueChanged += reminderPage.ReminderPageSearch;

            Debug.WriteLine("Subcribe to Filter1 Successful");
        }


        private void BSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchValueChanged?.Invoke(SearchValue);
        }
    }
}
