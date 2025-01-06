using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Mystic_ToDo.View.UserControls.CustomControls
{
    /// <summary>
    /// Interaction logic for SwitchWithLabelModied.xaml
    /// </summary>
    public partial class SwitchWithLabelModied : UserControl, INotifyPropertyChanged
    {
        public SwitchWithLabelModied()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(SwitchWithLabelModied), new PropertyMetadata(string.Empty));

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
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(SwitchWithLabelModied), new PropertyMetadata(false, OnIsCheckedChanged));

        

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set
            {
                SetValue(IsCheckedProperty, value);
                OnPropertyChanged();
            }
        }

        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SwitchWithLabelModied;
            if (control != null)
            {
                control.OnPropertyChanged(nameof(IsChecked));
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
