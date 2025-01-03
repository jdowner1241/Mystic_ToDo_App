using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mystic_ToDo.View.UserControls.Content.Calender.CalenderContent
{
    internal class YearSelectionViewModel : INotifyPropertyChanged
    {

        private int _selectedYear;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChangeYearCommand { get; }

        public YearSelectionViewModel()
        {
            SelectedYear = 2022; // Default selected year
            ChangeYearCommand = new RelayCommand(ChangeYear);
        }

        private void ChangeYear(object parameter)
        {
            if (parameter is int year)
            {
                SelectedYear = year;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
