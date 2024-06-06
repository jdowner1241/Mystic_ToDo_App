using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Mystic_ToDo.View.UserControls.CustomControls
{

    public partial class MenuBarButton : UserControl, INotifyPropertyChanged
    {
        public MenuBarButton()
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
    }
}
