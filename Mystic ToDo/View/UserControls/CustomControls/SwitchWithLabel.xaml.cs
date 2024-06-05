using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Mystic_ToDo.View.UserControls.CustomControls
{
    /// <summary>
    /// Interaction logic for SwitchWithLabel.xaml
    /// </summary>
    public partial class SwitchWithLabel : UserControl, INotifyPropertyChanged
    {
        public SwitchWithLabel()
        {
            DataContext = this;
            InitializeComponent();
        }

        private string placeholder;

        public event PropertyChangedEventHandler? PropertyChanged;

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

        private void HandleCheck(object sender, RoutedEventArgs e)
        {
                
        }

        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
           
        }


    }
}
