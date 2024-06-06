using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Mystic_ToDo.View.UserControls.Content
{
    /// <summary>
    /// Interaction logic for ReminderEditor.xaml
    /// </summary>
    public partial class ReminderEditor : UserControl
    {
        private ObservableCollection<string> alarmFreqencyList = new ObservableCollection<string> { "Daily", "Weekly", "Montly", "Yearly" };

        public ReminderEditor()
        {
            InitializeComponent();
            DataContext = this;
            setCboxObj();
        }

        public void setCboxObj()
        {
            cboxItems.CboxItems = alarmFreqencyList;
        }


    }
}
