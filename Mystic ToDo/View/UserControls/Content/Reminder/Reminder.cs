using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_ToDo.View.UserControls.Content.Reminder
{
    internal class Reminder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Alarm { get; set; }
        public bool HasAlarms { get; set; }
        public bool Periodic { get; set; }
        public bool TimeFrameSelection { get; set; }
    }
}
