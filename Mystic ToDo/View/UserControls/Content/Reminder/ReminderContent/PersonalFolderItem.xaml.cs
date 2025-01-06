using Mystic_ToDo.Data;
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
using static Mystic_ToDo.Database.ReminderDb;

namespace Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent
{
    /// <summary>
    /// Interaction logic for PersonalFolderItem.xaml
    /// </summary>
    public partial class PersonalFolderItem : UserControl, INotifyPropertyChanged
    {
        public PersonalFolderItem()
        {
            
            InitializeComponent();
            DataContext = this;

            Debug.Write($"\n\nFolder selection control initalize with UserID: {FolderId} \n\n");
        }

        private static PersonalFolderItem _lastSelectedItem;

        private string _folderName;
        private int _folderId;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action<int> SelectedFolder;
        public event Action<int, string> DeleteFolder;

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

                Debug.Write($"\n\nFolder selection control set with UserID: {FolderId} \n\n");
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Invoke the SelectedFolder event
            SelectedFolder?.Invoke(FolderId);

            // Change the background color of the selected item
            HighlightItem();

            // Change the background color of the selected item
            this.Background = Brushes.Blue;
            //Foldertbox.Background = Brushes.LightBlue;

            // Reset the background color of the previously selected item
            if (_lastSelectedItem != null && _lastSelectedItem != this) 
            { 
                _lastSelectedItem.Background = Brushes.Transparent;
                //Foldertbox.Background = Brushes.Transparent;
            }

            // Update the last selected item
            _lastSelectedItem = this;
        }


        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DeleteFolder?.Invoke(FolderId, FolderName);
        }

        public void HighlightItem()
        {
            // Change the background color of the selected item
            this.Background = Brushes.Blue;
            //Foldertbox.Background = Brushes.LightBlue;

            // Reset the background color of the previously selected item
            if (_lastSelectedItem != null && _lastSelectedItem != this)
            {
                _lastSelectedItem.Background = Brushes.Transparent;
                //Foldertbox.Background = Brushes.Transparent;
            }

            // Update the last selected item
            _lastSelectedItem = this;
        }
    }
}
