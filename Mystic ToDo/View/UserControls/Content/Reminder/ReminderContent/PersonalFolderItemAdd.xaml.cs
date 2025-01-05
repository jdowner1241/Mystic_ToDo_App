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
    /// Interaction logic for PersonalFolderItemAdd.xaml
    /// </summary>
    public partial class PersonalFolderItemAdd : UserControl, INotifyPropertyChanged
    {
        public PersonalFolderItemAdd()
        {
            InitializeComponent();
            DataContext = this;
        }

        private string _folderName;
        private int _folderId;

        public Action<string> AddNewFolder;
        public Action CancelNewFolder;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string FolderName
        {
            get { return _folderName; }
            set
            {
                _folderName = value;
                OnPropertyChanged();
            }
        }

        public int FolderId
        {
            get { return _folderId; }
            set
            {
                _folderId = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void bAdd_Click(object sender, RoutedEventArgs e)
        {
            AddNewFolder?.Invoke(FolderName);
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            CancelNewFolder?.Invoke();
        }
    }
}
