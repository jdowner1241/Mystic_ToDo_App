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
    /// Interaction logic for FilterItemsCbox.xaml
    /// </summary>
    public partial class FilterItemsCbox : UserControl, INotifyPropertyChanged
    {
        public FilterItemsCbox()
        {
            InitializeComponent();
            DataContext = this;
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                txtBlock.Text = value;
                OnPropertyChanged();
            }
        }

        public bool IsChecked
        {
            get { return chkBox.IsChecked == true; }
            set { chkBox.IsChecked = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Checked;
        public event EventHandler Unchecked;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            Checked?.Invoke(this, EventArgs.Empty);
        }

        private void ChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Unchecked?.Invoke(this, EventArgs.Empty);
        }
    }

}
