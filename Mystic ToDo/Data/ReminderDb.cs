using Mystic_ToDo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_ToDo.Database
{
    public class ReminderDb : BaseEntity
    {
        public enum TimeFrame
        {
            NotSet,
            Daily,
            Weekly,
            Monthly,
            Yearly
        }

        public class Reminder : BaseEntity
        {
            public string Name { get; set; }

            public string? Description { get; set; }

            public bool IsComplete { get; set; }

            public bool HasAlarms { get; set; }

            public DateTime? Alarm { get; set; }

            public bool Periodic { get; set; }

            public TimeFrame TimeFrameSelection { get; set; }

            public DateTime? PeriodicAlarm { get; set; }

            public string? UserId { get; set; }

        }

    }
}
