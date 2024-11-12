using Mystic_ToDo.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_ToDo.Database
{
    public class ReminderDb
    {
        public enum TimeFrameId
        {
            NotSet = 0,
            Daily = 1,
            Weekly = 2,
            Monthly = 3,
            Yearly = 4
        }

        public class Reminder
        {
            [Key]
            public int Id { get; set; }

            public string Name { get; set; }
            public string? Description { get; set; }
            public bool HasAlarms { get; set; }
            public DateTime? Alarm { get; set; }
            public bool Periodic { get; set; }

            public TimeFrameId TimeFrameId { get; set; } // Enum property

            [ForeignKey(nameof(TimeFrameId))]
            public TimeFrame TimeFrameSelection{get; set;}

            public DateTime? PeriodicAlarm { get; set; }
            public bool IsComplete { get; set; }
            public string? UserId { get; set; }
            public string? Folder {  get; set; }
        }

        public class TimeFrame
        {

            [Key]
            public ReminderDb.TimeFrameId TimeFrameId { get; set; }

            public string Name { get; private set; }

            public TimeFrame(ReminderDb.TimeFrameId timeFrameId)
            {
                TimeFrameId = timeFrameId;
                Name = timeFrameId.ToString();
            }

            // Parameterless constructor for EF
            public TimeFrame() { }
        }

    }
}
