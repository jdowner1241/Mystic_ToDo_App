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
        private bool isUpdating = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public DateTimePicker()
        {
            DataContext = this;
            InitializeComponent();
            timePicker.Visibility =  System.Windows.Visibility.Collapsed;
        }


        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePicker.SelectedDate != null)
            {
                timePicker.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                timePicker.Visibility = System.Windows.Visibility.Collapsed;
            }
            date = datePicker.SelectedDate;
            //UpdateDateTime();
        }

        private void TimePicker_SelectedTimeChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (date.HasValue) 
            {
                time = timePicker.Value?.TimeOfDay;
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

        public DateTime? DateWithTime
        {
            get { return dateWithTime; }
            set 
            {
                if (!isUpdating) 
                {
                    isUpdating = true;
                    dateWithTime = value;
                    OnPropertyChanged();

                    if (dateWithTime.HasValue)
                    {
                        datePicker.SelectedDate = dateWithTime.Value.Date;
                        timePicker.Value = dateWithTime.Value;
                        timePicker.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        datePicker.SelectedDate = null;
                        timePicker.Value = null;
                        datePicker.Visibility = System.Windows.Visibility.Collapsed;
                        timePicker.Visibility= System.Windows.Visibility.Collapsed;
                    }
                    isUpdating = false;
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateDateTime()
        {
            try
            {
                date = datePicker.SelectedDate;
                time = timePicker.Value?.TimeOfDay;

                if (date != null && time != null)
                {
                    DateWithTime = date.Value + time.Value;
                }
                else
                {
                    DateWithTime = null;
                }
            }
            catch { }
        }

        public DateTime? getDateTime()
        {
            UpdateDateTime(); 
            return DateWithTime;
        }

    }
}



