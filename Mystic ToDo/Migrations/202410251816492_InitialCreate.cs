namespace Mystic_ToDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
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
                        UserId = c.String(),
                        Folder = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TimeFrames", t => t.TimeFrameId, cascadeDelete: true)
                .Index(t => t.TimeFrameId);
            
            CreateTable(
                "dbo.TimeFrames",
                c => new
                    {
                        TimeFrameId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TimeFrameId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reminders", "TimeFrameId", "dbo.TimeFrames");
            DropIndex("dbo.Reminders", new[] { "TimeFrameId" });
            DropTable("dbo.TimeFrames");
            DropTable("dbo.Reminders");
        }
    }
}
