namespace Mystic_ToDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                {
                    UserId = c.Int(nullable: false, identity: true),
                    UserName = c.String(maxLength: 450),
                    EmailAddress = c.String(maxLength: 450),
                    Password = c.String(),
                })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.EmailAddress, unique: true);

            CreateTable(
                "dbo.Folders",
                c => new
                {
                    FolderId = c.Int(nullable: false, identity: true),
                    FolderName = c.String(maxLength: 450, nullable: false),
                    UserId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.FolderId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.FolderName, unique: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.TimeFrames",
                c => new
                {
                    TimeFrameId = c.Int(nullable: false),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.TimeFrameId);

            CreateTable(
                "dbo.Reminders",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Description = c.String(),
                    HasAlarms = c.Boolean(nullable: false),
                    Alarm = c.DateTime(),
                    Periodic = c.Boolean(nullable: false),
                    TimeFrameId = c.Int(nullable: false),
                    PeriodicAlarm = c.DateTime(),
                    IsComplete = c.Boolean(nullable: false),
                    UserId = c.Int(nullable: false),
                    FolderId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TimeFrames", t => t.TimeFrameId, cascadeDelete: true)
                .Index(t => t.TimeFrameId);

            CreateTable(
                "dbo.Calenders",
                c => new
                    {
                        CalenderId = c.Int(nullable: false, identity: true),
                        CalenderEventName = c.String(),
                        CalenderEventDate = c.DateTime(nullable: false),
                        CalenderEventTimeStart = c.Time(nullable: false, precision: 7),
                        CalenderEventTimeEnd = c.Time(nullable: false, precision: 7),
                        CalenderEventNotes = c.String(),
                    })
                .PrimaryKey(t => t.CalenderId);
            
            CreateTable(
                "dbo.DayInWeeks",
                c => new
                    {
                        DayInWeekId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.DayInWeekId);
            

            CreateTable(
                "dbo.TimetableDays",
                c => new
                    {
                        TimetableDayId = c.Int(nullable: false, identity: true),
                        DayInWeekId = c.Int(nullable: false),
                        TimetableEventStart = c.Time(nullable: false, precision: 7),
                        TimetableEventEnd = c.Time(nullable: false, precision: 7),
                        TimetableId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TimetableDayId)
                .ForeignKey("dbo.DayInWeeks", t => t.DayInWeekId, cascadeDelete: true)
                .ForeignKey("dbo.Timetables", t => t.TimetableId, cascadeDelete: true)
                .Index(t => t.DayInWeekId)
                .Index(t => t.TimetableId);
            
            CreateTable(
                "dbo.Timetables",
                c => new
                    {
                        TimetableId = c.Int(nullable: false, identity: true),
                        TimetableEventName = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TimetableId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TimeTrackerAlarms",
                c => new
                    {
                        TimeTrackerAlarmId = c.Int(nullable: false, identity: true),
                        AlarmName = c.String(),
                        AlarmDateTime = c.DateTime(nullable: false),
                        IsComplete = c.Boolean(nullable: false),
                        IsPeriodic = c.Boolean(nullable: false),
                        TimeFrameId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TimeTrackerAlarmId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.TimeFrames", t => t.TimeFrameId, cascadeDelete: true)
                .Index(t => t.TimeFrameId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TimeTrackerStopWatchLaps",
                c => new
                    {
                        TimeTrackerStopWatchLapId = c.Int(nullable: false, identity: true),
                        LapStartTime = c.Time(nullable: false, precision: 7),
                        LapEndTime = c.Time(nullable: false, precision: 7),
                        TimeTrackerStopWatchId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TimeTrackerStopWatchLapId)
                .ForeignKey("dbo.TimeTrackerStopWatches", t => t.TimeTrackerStopWatchId, cascadeDelete: true)
                .Index(t => t.TimeTrackerStopWatchId);
            
            CreateTable(
                "dbo.TimeTrackerStopWatches",
                c => new
                    {
                        TimeTrackerStopWatchId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TimeTrackerStopWatchId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TimeTrackerTimers",
                c => new
                    {
                        TimeTrackerTimerId = c.Int(nullable: false, identity: true),
                        TimeTrackerTimerName = c.String(),
                        CountdownDuration = c.Time(nullable: false, precision: 7),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TimeTrackerTimerId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            // Add foreign key constraints for Reminders table
            AddForeignKey("dbo.Reminders", "FolderId", "dbo.Folders", "FolderId", cascadeDelete: true); 
            AddForeignKey("dbo.Reminders", "UserId", "dbo.Users", "UserId", cascadeDelete: false);
        }

        public override void Down()
        {
            // Drop foreign key constraints
            DropForeignKey("dbo.Reminders", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reminders", "FolderId", "dbo.Folders");
            DropForeignKey("dbo.Reminders", "TimeFrameId", "dbo.TimeFrames");
            DropForeignKey("dbo.TimeTrackerTimers", "UserId", "dbo.Users");
            DropForeignKey("dbo.TimeTrackerStopWatchLaps", "TimeTrackerStopWatchId", "dbo.TimeTrackerStopWatches");
            DropForeignKey("dbo.TimeTrackerStopWatches", "UserId", "dbo.Users");
            DropForeignKey("dbo.TimeTrackerAlarms", "TimeFrameId", "dbo.TimeFrames");
            DropForeignKey("dbo.TimeTrackerAlarms", "UserId", "dbo.Users");
            DropForeignKey("dbo.TimetableDays", "TimetableId", "dbo.Timetables");
            DropForeignKey("dbo.Timetables", "UserId", "dbo.Users");
            DropForeignKey("dbo.TimetableDays", "DayInWeekId", "dbo.DayInWeeks");
            DropForeignKey("dbo.Folders", "UserId", "dbo.Users");

            // Drop indexes
            DropIndex("dbo.TimeTrackerTimers", new[] { "UserId" });
            DropIndex("dbo.TimeTrackerStopWatches", new[] { "UserId" });
            DropIndex("dbo.TimeTrackerStopWatchLaps", new[] { "TimeTrackerStopWatchId" });
            DropIndex("dbo.TimeTrackerAlarms", new[] { "UserId" });
            DropIndex("dbo.TimeTrackerAlarms", new[] { "TimeFrameId" });
            DropIndex("dbo.Timetables", new[] { "UserId" });
            DropIndex("dbo.TimetableDays", new[] { "TimetableId" });
            DropIndex("dbo.TimetableDays", new[] { "DayInWeekId" });
            DropIndex("dbo.Reminders", new[] { "UserId" });
            DropIndex("dbo.Reminders", new[] { "FolderId" });
            DropIndex("dbo.Reminders", new[] { "TimeFrameId" });
            DropIndex("dbo.Users", new[] { "EmailAddress" });
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.Folders", new[] { "UserId" });
            DropIndex("dbo.Folders", new[] { "FolderName" });

            // Drop tables
            DropTable("dbo.TimeTrackerTimers");
            DropTable("dbo.TimeTrackerStopWatches");
            DropTable("dbo.TimeTrackerStopWatchLaps");
            DropTable("dbo.TimeTrackerAlarms");
            DropTable("dbo.Timetables");
            DropTable("dbo.TimetableDays");
            DropTable("dbo.TimeFrames");
            DropTable("dbo.Reminders");
            DropTable("dbo.Users");
            DropTable("dbo.Folders");
            DropTable("dbo.DayInWeeks");
            DropTable("dbo.Calenders");
        }
    }
}
