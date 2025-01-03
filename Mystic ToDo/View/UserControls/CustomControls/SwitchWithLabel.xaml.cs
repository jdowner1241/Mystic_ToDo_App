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
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(SwitchWithLabel), new PropertyMetadata(string.Empty));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set
            {
                SetValue(PlaceholderProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(SwitchWithLabel), new PropertyMetadata(false));

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set
            {
                SetValue(IsCheckedProperty, value);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void HandleCheck(object sender, RoutedEventArgs e)
        {
            IsChecked = true;
        }

        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
            IsChecked = false;
        }
    }
}
