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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mystic_ToDo.View.UserControls.Content.LoginPage.LoginPageContent
{
    /// <summary>
    /// Interaction logic for LoginPageUser.xaml
    /// </summary>
    public partial class LoginPageUser : UserControl, INotifyPropertyChanged
    {
        public LoginPageUser()
        {
            DataContext = this;
            InitializeComponent();

            this.MouseEnter += this.OnMouseEnter;
            this.MouseLeave += this.OnMouseLeave;
            this.MouseLeftButtonDown += this.OnMouseLeftButtonDown;
        }

        private string username;
        private int userNumber;
        public event PropertyChangedEventHandler? PropertyChanged;

        public string UserName
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        public int UserNumber
        {
            get { return userNumber; }
            set { 
                userNumber = value; 
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
           background.Background = Brushes.LightBlue;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
           background.Background = Brushes.LightGray;
        }


        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty BackgroudProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(LoginPageUser), new PropertyMetadata(Brushes.LightGray));

        private void OnMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            
        }
    }
}
