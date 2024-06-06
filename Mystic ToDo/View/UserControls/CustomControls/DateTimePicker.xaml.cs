using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Core.Converters;


namespace Mystic_ToDo.View.UserControls.CustomControls
{
    /// <summary>
    /// Interaction logic for DateTimePicker.xaml
    /// </summary>
    public partial class DateTimePicker : UserControl, INotifyPropertyChanged
    {
        private string placeholder;
        private DateTime? date;
        private TimeSpan? time;
        private DateTime? dateWithTime;

        public event PropertyChangedEventHandler? PropertyChanged;

        public DateTimePicker()
        {
            DataContext = this;
            InitializeComponent();
            timePicker.Visibility =  System.Windows.Visibility.Collapsed;
            timePickerPlaceholder();
            
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePicker.SelectedDate != null)
            {
                timePicker.Visibility = System.Windows.Visibility.Visible;
            }else
            {
                timePicker.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void timePickerPlaceholder()
        {
            DateTime? d;
            TimeSpan? t;
            DateTime? dt;

            if (datePicker.SelectedDate.HasValue) 
            {
                d = datePicker.SelectedDate.Value;
            }else
            {
                d = DateTime.Today;
            }
            
            t = new TimeSpan(0, 0, 0, 0);
            dt = d + t;

            if (dt.HasValue)
            {
                timePicker.Value = dt.Value;
            }

        }

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

        private void setDateTime()
        {
            try
            {
                date = datePicker.SelectedDate;
                time = timePicker.Value?.TimeOfDay;

                if (date != null && time != null)
                {
                    dateWithTime = date.Value + time.Value;
                }
                else
                {
                    MessageBox.Show("Missing or Incomplete Date/Time selection.");
                }

            }
            catch { }
        }

    }
}



