namespace Mystic_ToDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecreateFoldersTable : DbMigration
    {
        public override void Up()
        {
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

            DropForeignKey("dbo.Reminders", "FolderId", "dbo.Folders");
            DropForeignKey("dbo.Reminders", "UserId", "dbo.Users");
            DropIndex("dbo.Reminders", new[] { "FolderId" });
            DropIndex("dbo.Reminders", new[] { "UserId" });

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

            // Remove redundant operations 
            // AddColumn("dbo.Folders", "UserId", c => c.Int(nullable: false)); 
            // AlterColumn("dbo.Folders", "FolderName", c => c.String(maxLength: 450));
            AlterColumn("dbo.Reminders", "FolderId", c => c.Int(nullable: false));
            AlterColumn("dbo.Reminders", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Folders", "FolderName", unique: true); 
            CreateIndex("dbo.Folders", "UserId"); 
            CreateIndex("dbo.Reminders", "FolderId"); 
            CreateIndex("dbo.Reminders", "UserId"); 
            AddForeignKey("dbo.Folders", "UserId", "dbo.Users", "UserId", cascadeDelete: true); 
            AddForeignKey("dbo.Reminders", "FolderId", "dbo.Folders", "FolderId", cascadeDelete: true);
            AddForeignKey("dbo.Reminders", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Reminders", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reminders", "FolderId", "dbo.Folders");
            DropForeignKey("dbo.TimeTrackerTimers", "UserId", "dbo.Users");
            DropForeignKey("dbo.TimeTrackerStopWatchLaps", "TimeTrackerStopWatchId", "dbo.TimeTrackerStopWatches");
            DropForeignKey("dbo.TimeTrackerStopWatches", "UserId", "dbo.Users");
            DropForeignKey("dbo.TimeTrackerAlarms", "TimeFrameId", "dbo.TimeFrames");
            DropForeignKey("dbo.TimeTrackerAlarms", "UserId", "dbo.Users");
            DropForeignKey("dbo.TimetableDays", "TimetableId", "dbo.Timetables");
            DropForeignKey("dbo.Timetables", "UserId", "dbo.Users");
            DropForeignKey("dbo.TimetableDays", "DayInWeekId", "dbo.DayInWeeks");
            DropForeignKey("dbo.Folders", "UserId", "dbo.Users");

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
            DropIndex("dbo.Folders", new[] { "UserId" });
            DropIndex("dbo.Folders", new[] { "FolderName" });

            AlterColumn("dbo.Reminders", "UserId", c => c.Int());
            AlterColumn("dbo.Reminders", "FolderId", c => c.Int());
            AlterColumn("dbo.Folders", "FolderName", c => c.String());

            DropColumn("dbo.Folders", "UserId");
            DropTable("dbo.TimeTrackerTimers");
            DropTable("dbo.TimeTrackerStopWatches");
            DropTable("dbo.TimeTrackerStopWatchLaps");
            DropTable("dbo.TimeTrackerAlarms");
            DropTable("dbo.Timetables");
            DropTable("dbo.TimetableDays");
            DropTable("dbo.DayInWeeks");
            DropTable("dbo.Calenders");

            CreateIndex("dbo.Reminders", "UserId");
            CreateIndex("dbo.Reminders", "FolderId");
            AddForeignKey("dbo.Reminders", "UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Reminders", "FolderId", "dbo.Folders", "FolderId");
        }
    }
}
