using Mystic_ToDo.Data;
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

namespace Mystic_ToDo.View.UserControls.Content.LoginPage
{
    /// <summary>
    /// Interaction logic for LoginPage1.xaml
    /// </summary>
    public partial class LoginPage : UserControl, INotifyPropertyChanged
    {
        private ReminderContext _context;

        public LoginPage()
        {
            DataContext = this;
            InitializeComponent();

            _context = new ReminderContext();
        }

        private string _userName;
        private int _usernumber;
        public event PropertyChangedEventHandler PropertyChanged;

        public string UserName
        {
            get { return _userName; }
            set
            {
                using (var db = new ReminderContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.UserId == UserNumber); 
                    if (user != null) 
                    { 
                        _userName = user.UserName; 
                    }
                    else
                    {
                        _userName = value; // If user is not found, set the provided value
                    } 
                } 
                OnPropertyChanged(); 
            }
        }

        public int UserNumber
        {
            get { return _usernumber; }
            set
            {
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
