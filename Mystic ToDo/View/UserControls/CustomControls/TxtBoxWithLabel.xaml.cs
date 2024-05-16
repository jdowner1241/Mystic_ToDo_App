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

namespace Mystic_ToDo.View.UserControls.CustomControls
{
    /// <summary>
    /// Interaction logic for txtBoxWithLabel.xaml
    /// </summary>
    public partial class TxtBoxWithLabel : UserControl
    {
        public TxtBoxWithLabel()
        {
            InitializeComponent();
        }

        private string placeholder;

        public string Placeholder
        {
            get { return placeholder; }
            set
            {
                placeholder = value;
                txtBoxLabel.Text = placeholder;
            }
        }

        private void txtBoxClear_Click(object sender, RoutedEventArgs e)
        {
            //if(txtBox = string.IsNullOrEmpty)
            txtBox.Text = string.Empty;
            txtBox.Focus();
        }
    }
}
