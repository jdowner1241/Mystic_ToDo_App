using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
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
    /// Interaction logic for PersonalFolder1.xaml
    /// </summary>
    public partial class PersonalFolder1 : UserControl, INotifyPropertyChanged
    {
        public PersonalFolder1()
        {
            DataContext = this;
            InitializeComponent();

            LoadFolderList();
        }

        private int _userId;
        private int _selectedFolderId;

        public event PropertyChangedEventHandler PropertyChanged;

        public int UserId
        {
            get { return _userId; }
            set 
            { 
                _userId = value;
                OnPropertyChanged();
            }
        }

        public int SelectedFolderId
        {
            get { return _selectedFolderId; }
            set 
            { 
                _selectedFolderId = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Fetch folder from the database for a specific user ID
        private List<ReminderDb.Folder> FetchFolders()
        {
            if (UserId != 0)
            {
                using (var context = new ReminderContext())
                {
                    var folders = context.Folders
                                            .Where(folders => folders.UserId == UserId)
                                            .ToList();
                    return folders;
                }
            }
            else
            {
                Debug.WriteLine("User not set");
                return new List<ReminderDb.Folder>(); // Return an empty list to handle the case where UserId is not set
            }
        }

        //Create a FolderList from the folders
        private void UpdateUI(IEnumerable<ReminderDb.Folder> folders)
        {

            UIFolderList.Children.Clear();

            foreach (var folder in folders)
            {
                if (folder != null)
                {
                    PersonalFolderItem newFolder= new PersonalFolderItem();
                    newFolder.FolderId = folder.FolderId;
                    newFolder.FolderName = folder.FolderName;
                    newFolder.SelectedFolder += OnFolderSelection;
                    UIFolderList.Children.Add(newFolder);
                }
            }      
        }

        // Load folderList
        private void LoadFolderList()
        {
            var FolderList = FetchFolders();
            UpdateUI (FolderList);
        }

        private void OnFolderSelection(int selectedFolderId)
        {
            SelectedFolderId = selectedFolderId;
        }

        private void bAddUser_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
