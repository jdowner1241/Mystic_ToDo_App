using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Mystic_ToDo.View.UserControls.CustomControls
{
    /// <summary>
    /// Interaction logic for DatePicker.xaml
    /// </summary>
    public partial class DatePicker : UserControl, INotifyPropertyChanged
    {
        private string placeholder;
        private TimeSpan? time;
        private DateTime? date;
        private DateTime? dateWithTime;
        private int hr;
        private int mm;
        private int ampm;

        public event PropertyChangedEventHandler? PropertyChanged;


        public DatePicker()
        {
            DataContext = this;
            InitializeComponent();
            showCboxItems();
        }

    
        private void showCboxItems()
        {
            showhr();
            showmm();
            showampm();
        }

        private void showhr()
        {
            List<int> hour = new List <int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
            timePickerHr.ItemsSource = hour;

            timePickerHr.SelectedItem = 1;
        }

        private void showmm()
        {
            List<string> mins = new List<string>();

            for (int i = 0; i <= 60; i++)
            {
                string formatedtime = String.Format("{0:00}", i);
                mins.Add(formatedtime);
            }
            timePickerMin.ItemsSource = mins;

            timePickerMin.SelectedItem = "00";
        }

        private void showampm()
        {
            List<string> ampm = new List<string> { "AM", "PM"};
            timePickerClock.ItemsSource = ampm;

            timePickerClock.SelectedItem = "AM";
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

                hr = Convert.ToInt32(timePickerHr);
                mm = Convert.ToInt32(timePickerMin);

                if ((string)timePickerClock.SelectedItem == "AM")
                {
                    if (hr == 12)
                    {
                        hr = 0;
                        ampm = 0;
                    }
                    else
                    {
                        ampm = 0;
                    }
                }
                else
                {
                    if (hr == 12 && mm > 0)
                    {
                        ampm = 0;
                        hr = 0;
                    }
                    else
                    {
                        ampm = 12;
                    }
                }

                time = new TimeSpan((hr + ampm), mm, 0);
                date = datePicker.SelectedDate;

                if (date != null && time != null)
                {
                    dateWithTime = date + time;
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
