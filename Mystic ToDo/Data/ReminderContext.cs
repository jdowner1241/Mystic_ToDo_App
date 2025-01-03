using Mystic_ToDo.Database;
using Mystic_ToDo.Migrations;
using Mystic_ToDo.View.UserControls.Content.Reminder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using static Mystic_ToDo.Database.ReminderDb;

namespace Mystic_ToDo.Data
{
    public class ReminderContext : DbContext
    {

        public DbSet<ReminderDb.User> Users { get; set; }
        public DbSet<ReminderDb.Reminder> Reminders { get; set; }
        public DbSet<ReminderDb.TimeFrame> TimeFrames { get; set; }
        public DbSet<ReminderDb.Folder> Folders { get; set; }
        public DbSet<FolderPerUser> FoldersPerUser { get; set; }
        public DbSet<ReminderDb.Calender> Calenders { get; set; }
        public DbSet<ReminderDb.Timetable> Timetables { get; set; }
        public DbSet<ReminderDb.TimetableDay> TimetableDays { get; set; }
        public DbSet<ReminderDb.DayInWeek> DayInWeeks { get; set; }
        public DbSet<ReminderDb.TimeTrackerStopWatch> TimeTrackerStopWatchs { get; set; }
        public DbSet<ReminderDb.TimeTrackerStopWatchLap> TimeTrackerStopWatchLaps { get; set; }
        public DbSet<TimeTrackerAlarm> TimeTrackerAlarms { get; set; }
        public DbSet<TimeTrackerTimer> TimeTrackerTimers { get; set; }

        private ReminderPage _reminderPage;

        public event EventHandler ReminderDataChanged;
        public event EventHandler UserDataChanged;


/*
        public override int SaveChanges()
        { // Logic to handle FolderNumberPerUser increment
          foreach (var entry in ChangeTracker.Entries<Folder>()) 
            { if (entry.State == EntityState.Added) 
                { 
                    var userId = entry.Entity.UserId; 
                    var maxFolderNumber = FoldersPerUser 
                            .Where(fpu => fpu.UserId == userId) 
                            .Select(fpu => fpu.FolderNumberPerUser)
                            .DefaultIfEmpty(0) 
                            .Max();
                    
                    var folderPerUser = new FolderPerUser 
                            { 
                                UserId = userId, 
                                FolderId = entry.Entity.FolderId, 
                                FolderNumberPerUser = maxFolderNumber + 1 
                            }; 
                    FoldersPerUser.Add(folderPerUser);
                } 
            } 
            return base.SaveChanges(); 
        }*/







        public void NotifyReminderDataChanged()
        {
            Debug.WriteLine("Notifying data change");
            ReminderDataChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SaveReminder(ReminderDb.Reminder reminder)
        {
            Reminders.Add(reminder);
            SaveChanges();
            NotifyReminderDataChanged();
        }

        public void NotifyUserDataChanged()
        {
            Debug.WriteLine("Notifying data change");
            UserDataChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SaveUser(ReminderDb.User user)
        {
            Users.Add(user);
            SaveChanges();
            NotifyUserDataChanged();
        }

       
       

    }
}
