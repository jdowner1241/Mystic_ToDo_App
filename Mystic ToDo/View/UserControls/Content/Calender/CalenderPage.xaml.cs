using System;
using System.Collections.Generic;
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

namespace Mystic_ToDo.View.UserControls.Content.Calender
{
    /// <summary>
    /// Interaction logic for CalenderPage.xaml
    /// </summary>
    public partial class CalenderPage : UserControl
    {
        public CalenderPage()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}
