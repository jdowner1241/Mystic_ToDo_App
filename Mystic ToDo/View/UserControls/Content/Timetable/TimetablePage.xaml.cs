using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace Mystic_ToDo.View.UserControls.Content.Timetable
{
    /// <summary>
    /// Interaction logic for TimetablePage.xaml
    /// </summary>
    public partial class TimetablePage : UserControl
    {
        public TimetablePage()
        {
            DataContext = this;
            InitializeComponent();
        }

        public class TimetableViewModel
        {
            public ObservableCollection<TimetableItem> TimetableItems { get; set; }

            public TimetableViewModel()
            {
                TimetableItems = new ObservableCollection<TimetableItem>
            {
                new TimetableItem { Day = "Monday", Time = "8:00 AM - 9:00 AM", Activity = "Mathematics Class", Notes = "Room A101" },
                new TimetableItem { Day = "Monday", Time = "9:00 AM - 10:00 AM", Activity = "English Class", Notes = "Room B202" },
                new TimetableItem { Day = "Monday", Time = "10:00 AM - 11:00 AM", Activity = "Go to the Gym", Notes = string.Empty },
                new TimetableItem { Day = "Tuesday", Time = "8:00 AM - 9:00 AM", Activity = "Important Meeting", Notes = "Office" },
                new TimetableItem { Day = "Tuesday", Time = "9:00 AM - 10:00 AM", Activity = "Science Lab", Notes = "Room C303" },
                new TimetableItem { Day = "Tuesday", Time = "11:00 AM - 12:00 PM", Activity = "Meet my girlfriend", Notes = string.Empty },
                new TimetableItem { Day = "Tuesday", Time = "2:00 PM - 4:00 PM", Activity = "Online Presentation", Notes = string.Empty }
            };
            }
        }

        public class TimetableItem
        {
            public string Day { get; set; }
            public string Time { get; set; }
            public string Activity { get; set; }
            public string Notes { get; set; }
        }


    }
}
