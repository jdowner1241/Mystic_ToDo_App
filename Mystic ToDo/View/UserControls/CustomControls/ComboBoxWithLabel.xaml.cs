using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mystic_ToDo.View.UserControls.CustomControls
{
    /// <summary>
    /// Interaction logic for ComboBoxWithLabel.xaml
    /// </summary>
    public partial class ComboBoxWithLabel : UserControl, INotifyPropertyChanged
    {
        public ComboBoxWithLabel()
        {
            DataContext = this;
            InitializeComponent();
            addComboboxItems(CboxItems);
        }


        private string placeholder;
        private Color cboxBColor;
        private ObservableCollection<string> cboxItems = new ObservableCollection<string>();
 

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

        public Color CboxBColor
        {
            get { return cboxBColor; }
            set
            {
                cboxBColor = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> CboxItems
        {
            get => cboxItems;

            set
            {
                cboxItems = value;
                OnPropertyChanged();
            }
        }

        public void addComboboxItems(ObservableCollection<string> CboxItems)
        {
            try
            {
                if (CboxItems != null)
                {
                    foreach (string i in CboxItems)
                    {
                        ComboBoxItem item = new ComboBoxItem { Content = i };

                        comboBox.Items.Add(item);
                    }
                }

                comboBox.SelectedIndex = 0;

            }
            catch 
            {
                Exception ex = null;
            }
            
        }

        public void setBackgroundColor(Color color)
        {

        }


        private void OnPropertyChanged([CallerMemberName] string propertyName= null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
