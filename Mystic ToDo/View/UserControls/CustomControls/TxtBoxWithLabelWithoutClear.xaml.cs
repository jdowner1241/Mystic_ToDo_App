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

namespace Mystic_ToDo.View.UserControls.CustomControls
{
    /// <summary>
    /// Interaction logic for TxtBoxWithLabelWithoutClear.xaml
    /// </summary>
    public partial class TxtBoxWithLabelWithoutClear : UserControl, INotifyPropertyChanged
    {
        public TxtBoxWithLabelWithoutClear()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(TxtBoxWithLabelWithoutClear), new PropertyMetadata(string.Empty));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set
            {
                SetValue(PlaceholderProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty TextValueProperty =
            DependencyProperty.Register("TextValue", typeof(string), typeof(TxtBoxWithLabelWithoutClear), new PropertyMetadata(string.Empty));

        public string TextValue
        {
            get { return (string)GetValue(TextValueProperty); }
            set
            {
                SetValue(TextValueProperty, value);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
