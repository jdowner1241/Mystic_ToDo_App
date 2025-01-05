using Mystic_ToDo.Data;
using Mystic_ToDo.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
           
            InitializeComponent();
            DataContext = this;

            Debug.Write($"\n\nPersonalFolder1 with UserID: {UserId} \n\n");

            NewFolderToggle = false;
            LoadFolderList();

            // Initialize the initial folder selection
            InitailFolderSelection();
        }

        private int _userId;
        private int _selectedFolderId;
        private bool NewFolderToggle { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public event Action<int> SelectedFolderIDUpdate;

        public int UserId
        {
            get { return _userId; }
            set 
            { 
                _userId = value;
                OnPropertyChanged();
                Debug.Write($"\n\nPersonalFolder1 set UserID: {UserId} \n\n");
            }
        }

        public int SelectedFolderId
        {
            get { return _selectedFolderId; }
            set 
            { 
                _selectedFolderId = value;
                OnPropertyChanged();
                //SelectedFolderIDUpdate(SelectedFolderId);
                SelectedFolderIDUpdate?.Invoke(SelectedFolderId);
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
                    PersonalFolderItem newFolder= new PersonalFolderItem()
                    {
                        FolderId = folder.FolderId,
                        FolderName = folder.FolderName
                    };
                    newFolder.SelectedFolder += OnFolderSelection;
                    newFolder.DeleteFolder += OnFolderDelete;
                    UIFolderList.Children.Add(newFolder);
                }
            }
            if (NewFolderToggle)
            {
                PersonalFolderItemAdd AddNewFolder = new PersonalFolderItemAdd();
                AddNewFolder.AddNewFolder += OnFolderAdd;
                AddNewFolder.CancelNewFolder += OnFolderCancel;
                UIFolderList.Children.Add(AddNewFolder);
            }
        }

        // Load folderList
        public void LoadFolderList()
        {
            var FolderList = FetchFolders();
            UpdateUI (FolderList);
            InitailFolderSelection();
        }

        // Load FolderList and add a new folder to add
        public void OnFolderAdd(string newFolderName)
        {
            //add newfolder
            using (var context = new ReminderContext())
            {
                UserService.AddNewFolderForUser(context, UserId, newFolderName);
            }  

            //Reset folderlist to default. 
            NewFolderToggle = false;
            LoadFolderList();
        }

        public void OnFolderCancel()
        {
            //Reset folderlist to default. 
            NewFolderToggle = false;
            LoadFolderList();
        }

        private void InitailFolderSelection()
        {
            using (var context = new ReminderContext())
            {
                var defaultFolder = context.FoldersPerUser.FirstOrDefault(fpu => fpu.UserId == UserId && fpu.FolderNumberPerUser == 1); 
                
                if (defaultFolder != null)
                {
                    SelectedFolderId = defaultFolder.FolderId; // Send an event to PersonalFolderItem to highlight the initial folder
                    SelectedFolderIDUpdate?.Invoke(SelectedFolderId); 
                }

                // Find the corresponding PersonalFolderItem and highlight it
                foreach (PersonalFolderItem folderItem in UIFolderList.Children)
                { 
                    if (folderItem.FolderId == defaultFolder.FolderId) 
                    { 
                        folderItem.HighlightItem(); 
                        break; 
                    } 
                }
            }
        }

        private void OnFolderSelection(int selectedFolderId)
        {
            SelectedFolderId = selectedFolderId;
            Debug.Write($"\n\nFolder ID: {selectedFolderId} \n\n");
        }

       

        private void bAddUser_Click(object sender, RoutedEventArgs e)
        {
            NewFolderToggle = true;
            LoadFolderList();
        }

        private void OnFolderDelete(int selectedFolderId, string folderName)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show(
                $" Do you want to Delete this Folder? \n Folder Name: {folderName}",
                "Delete Folder !!!",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning
            );
            if (result == MessageBoxResult.OK)
            {
                using (var context = new ReminderContext())
                {
                    var resultMessage = UserService.RemoveFolderForUser(context, UserId, selectedFolderId); MessageBox.Show(resultMessage);
                }
            }
            else
            {
                MessageBox.Show("Folder delete canceled");
            }

            LoadFolderList();
        }

    }
}
