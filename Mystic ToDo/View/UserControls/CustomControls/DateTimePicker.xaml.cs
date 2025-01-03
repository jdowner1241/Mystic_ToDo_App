using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace Mystic_ToDo.View.UserControls.CustomControls
{
    /// <summary>
    /// Interaction logic for DateTimePicker.xaml
    /// </summary>
    public partial class DateTimePicker : UserControl, INotifyPropertyChanged
    {
        private bool isUpdating = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public DateTimePicker()
        {
            InitializeComponent();
            DataContext = this;
            timePicker.Visibility = Visibility.Collapsed;
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(DateTimePicker), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty DateWithTimeProperty =
            DependencyProperty.Register("DateWithTime", typeof(DateTime?), typeof(DateTimePicker), new PropertyMetadata(null, OnDateWithTimeChanged));

        private static void OnDateWithTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (DateTimePicker)d;
            control.UpdateUI();
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set
            {
                SetValue(PlaceholderProperty, value);
                OnPropertyChanged();
            }
        }

        public DateTime? DateWithTime
        {
            get { return (DateTime?)GetValue(DateWithTimeProperty); }
            set
            {
                SetValue(DateWithTimeProperty, value);
                OnPropertyChanged();
                UpdateUI();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePicker.SelectedDate != null)
            {
                timePicker.Visibility = Visibility.Visible;
            }
            else
            {
                timePicker.Visibility = Visibility.Collapsed;
            }
            UpdateDateTime();
        }

        private void TimePicker_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateDateTime();
        }


        private void UpdateDateTime()
        {
            if (!isUpdating)
            {
                isUpdating = true;
                try
                {
                    var date = datePicker.SelectedDate;
                    var time = timePicker.Value?.TimeOfDay;

                    if (date != null && time != null)
                    {
                        DateWithTime = date.Value + time.Value;
                    }
                    else
                    {
                        DateWithTime = null;
                    }
                }
                finally
                {
                    isUpdating = false;
                }
            }
        }

        private void UpdateUI()
        {
            if (!isUpdating)
            {
                isUpdating = true;
                try
                {
                    if (DateWithTime.HasValue)
                    {
                        datePicker.SelectedDate = DateWithTime.Value.Date;
                        timePicker.Value = DateWithTime.Value;
                        timePicker.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        datePicker.SelectedDate = null;
                        timePicker.Value = null;
                        timePicker.Visibility = Visibility.Collapsed;
                    }
                }
                finally
                {
                    isUpdating = false;
                }
            }
        }
    }
}
