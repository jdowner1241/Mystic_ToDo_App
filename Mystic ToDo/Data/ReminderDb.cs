using Mystic_ToDo.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Mystic_ToDo.Database
{
    public class ReminderDb
    {

        public class User
        {
            [Key]
            public int UserId { get; set; }

            [Index(IsUnique = true)]
            [MaxLength(450)]
            public string UserName { get; set; }

            [Index(IsUnique = true)]
            [MaxLength(450)]
            public string EmailAddress { get; set; }

            public string Password { get; set; }

            public List<Folder> Folders { get; set; } = new List<Folder>(); 
            
            public List<FolderPerUser> FolderPerUsers { get; set; } = new List<FolderPerUser>();
        }


        //Reminder Page info
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

            public int UserId { get; set; }
            [ForeignKey(nameof(UserId))]
            public User SelectedUser { get; set; }

            public int FolderId { get; set; }
            [ForeignKey(nameof(FolderId))]
            public Folder SelectedFolder { get; set; }
        }

        public enum TimeFrameId
        {
            NotSet = 0,
            Daily = 1,
            Weekly = 2,
            Monthly = 3,
            Yearly = 4
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

        public class FolderPerUser
        {
            [Key]
            public int FolderPerUserId { get; set; }

            public int UserId { get; set; }
            [ForeignKey(nameof(UserId))]
            public User User { get; set; }

            public int FolderId { get; set; }
            [ForeignKey(nameof(FolderId))] 
            public Folder Folder { get; set; }

            public int FolderNumberPerUser { get; set; }
        }

        public class Folder
        {
            [Key]
            public int FolderId { get; set; }

            [MaxLength(450)]
            public string FolderName { get; set; }

            public int UserId { get; set; }
            [ForeignKey(nameof(UserId))]
            public User SelectedUser { get; set; }

            public List<FolderPerUser> FolderPerUserList { get; set; } = new List<FolderPerUser>();
        }


        //Calender Page info
        public class Calender
        {
            [Key]
            public int CalenderId { get; set; }

            public string CalenderEventName { get; set; }  
            public DateTime CalenderEventDate { get; set; }

            public TimeSpan CalenderEventTimeStart { get; set; }
            public TimeSpan CalenderEventTimeEnd { get; set; }

            public String CalenderEventNotes { get; set; }
        }

        //Calender Page info
        public class Timetable
        {
            [Key]
            public int TimetableId { get; set; }

            public string TimetableEventName { get; set; }

            public List<TimetableDay> TimetableDays { get; set; } = new List<TimetableDay>();

            public int UserId { get; set; }
            [ForeignKey(nameof(UserId))]
            public User SelectedUser { get; set; }

        }

        public class TimetableDay
        {
            [Key]
            public int TimetableDayId { get; set; }

            public DayInWeekId DayInWeekId { get; set; } // Enum property
            [ForeignKey(nameof(DayInWeekId))]
            public DayInWeek DaySelection { get; set; }

            public TimeSpan TimetableEventStart { get; set; }
            public TimeSpan TimetableEventEnd { get; set; }

            public int TimetableId {get; set;}
            [ForeignKey(nameof(TimetableId))]
            public Timetable TimetableEvent { get; set; }
        }

        public enum DayInWeekId
        {
            Sunday = 0,
            Monday = 1,
            Tuesday = 2,
            Wednesday = 3,
            Thursday = 4,
            Friday = 5,
            Saturday = 6
        }

        public class DayInWeek
        {
            [Key]
            public ReminderDb.DayInWeekId DayInWeekId { get; set; }

            public string Name { get; private set; }

            public DayInWeek(ReminderDb.DayInWeekId dayInWeekId)
            {
                DayInWeekId = dayInWeekId;
                Name = dayInWeekId.ToString();
            }

            // Parameterless constructor for EF
            public DayInWeek() { }
        }

        //TimeTracker Page info
        public class TimeTrackerStopWatch
        {
            [Key]
            public int TimeTrackerStopWatchId { get; set; }

            public List<TimeTrackerStopWatchLap> TimeTrackerStopWatchLaps { get; set; } = new List<TimeTrackerStopWatchLap>();
            
            public int UserId { get; set; }
            [ForeignKey(nameof(UserId))]
            public User SelectedUser { get; set; }
        }

        public class TimeTrackerStopWatchLap
        {
            [Key]
            public int TimeTrackerStopWatchLapId { get; set; }

           
            public TimeSpan LapStartTime { get; set; }
            public TimeSpan LapEndTime { get; set; }

            public int TimeTrackerStopWatchId { get; set; }
            [ForeignKey(nameof(TimeTrackerStopWatchId))]
            public TimeTrackerStopWatch TimeTrackerStopWatch { get; set; }
        }


        public class TimeTrackerAlarm
        {
            [Key]
            public int TimeTrackerAlarmId { get; set; }
            public string? AlarmName { get; set; }

            public DateTime AlarmDateTime { get; set; }

            public bool IsComplete { get; set; }
            public bool IsPeriodic { get; set; }

            public TimeFrameId TimeFrameId { get; set; } // Enum property
            [ForeignKey(nameof(TimeFrameId))]
            public TimeFrame TimeFrameSelection { get; set; }

            public int UserId { get; set; }
            [ForeignKey(nameof(UserId))]
            public User SelectedUser { get; set; }
        }

        public class TimeTrackerTimer
        {
            [Key]
            public int TimeTrackerTimerId { get; set; }

            public string? TimeTrackerTimerName { get; set; }

            public TimeSpan CountdownDuration { get; set; } // Time interval for countdown

            public DateTime? StartTime { get; set; } // Optional: when the countdown starts
            public DateTime? EndTime { get; set; } // Optional: when the countdown ends

            public int UserId { get; set; }
            [ForeignKey(nameof(UserId))]
            public User SelectedUser { get; set; }
        }


        /*public class MigrationHistory 
        { 
            [Key] 
            public int Id { get; set; }

            [Index(IsUnique = true)]
            [MaxLength(450)]
            public string Migration { get; set; } 
        }*/

    }
}
