namespace Mystic_ToDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Folders",
                c => new
                    {
                        FolderId = c.Int(nullable: false, identity: true),
                        FolderName = c.String(),
                    })
                .PrimaryKey(t => t.FolderId);
            
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
                        FolderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Folders", t => t.FolderId, cascadeDelete: true)
                .ForeignKey("dbo.TimeFrames", t => t.TimeFrameId, cascadeDelete: true)
                .Index(t => t.TimeFrameId)
                .Index(t => t.FolderId);
            
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
            DropForeignKey("dbo.Reminders", "FolderId", "dbo.Folders");
            DropIndex("dbo.Reminders", new[] { "FolderId" });
            DropIndex("dbo.Reminders", new[] { "TimeFrameId" });
            DropTable("dbo.TimeFrames");
            DropTable("dbo.Reminders");
            DropTable("dbo.Folders");
        }
    }
}
