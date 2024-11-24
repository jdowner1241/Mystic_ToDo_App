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
    /// Interaction logic for PersonalFolderItem.xaml
    /// </summary>
    public partial class PersonalFolderItem : UserControl, INotifyPropertyChanged
    {
        private string placeholder;

        public event PropertyChangedEventHandler? PropertyChanged;

        public PersonalFolderItem()
        {
            DataContext = this;
            InitializeComponent();
        }

        public string Placeholder
        {
            get { return placeholder; }
            set
            {
                placeholder = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}
