﻿using System;
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
    }
}
