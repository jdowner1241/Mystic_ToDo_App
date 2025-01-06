using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for FilterItems.xaml
    /// </summary>
    public partial class FilterItems : UserControl, INotifyPropertyChanged
    {
        public FilterItems()
        {
            InitializeComponent();
            DataContext = this;
        }

        private string _header;
        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
                OnPropertyChanged();
            }
        }

        /*public void AddFilterItem(string text)
        {
            var newItem = new FilterItemsCbox { Text = text };
            newItem.Margin = new Thickness(0, 5, 0, 5);

            if (stackPanelLeft.Children.Count <= stackPanelRight.Children.Count)
            {
                stackPanelLeft.Children.Add(newItem);
            }
            else
            {
                stackPanelRight.Children.Add(newItem);
            }
        }*/

        public void AddFilterItem(FilterItemsCbox item) 
        { 
            item.Margin = new Thickness(0, 5, 0, 5);
            
            if (stackPanelLeft.Children.Count <= stackPanelRight.Children.Count) 
            { 
                stackPanelLeft.Children.Add(item);
            } 
            else
            { 
                stackPanelRight.Children.Add(item); 
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
