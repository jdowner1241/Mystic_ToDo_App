namespace Mystic_ToDo.Migrations
{
    using Mystic_ToDo.Data;
    using Mystic_ToDo.Database;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Mystic_ToDo.Data.ReminderContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }



        protected override void Seed(Mystic_ToDo.Data.ReminderContext context)
        {
            // Seed TimeFrame data from enum
            foreach (var value in Enum.GetValues(typeof(ReminderDb.TimeFrameId)))
            {
                var timeFrameId = (ReminderDb.TimeFrameId)value;
                context.TimeFrames.AddOrUpdate(
                    tf => tf.TimeFrameId,
                    new ReminderDb.TimeFrame(timeFrameId) // Use constructor to set Name
                );
            }

            // Ensure the Guest user and their Default folder are created
            if (!context.Users.Any(u => u.UserId == 0) || !context.Users.Any(u => u.UserId == 1) ) 
            { 
                UserService.CreateInitialUser(context, "Guest", string.Empty, string.Empty); 
            }





            /*if (!context.Users.Any(u => u.UserId == 0 ) || !context.Users.Any(u => u.UserId == 1)) 
            { 
                context.Users.AddOrUpdate(
                    u => u.UserId, 
                    new ReminderDb.User { UserId = 1, UserName = "Guest", EmailAddress = string.Empty, Password = string.Empty });

                context.SaveChanges();

                if (!context.Folders.Any(f => f.FolderId == 0) || !context.Folders.Any(f => f.FolderId == 1))
                {
                    context.Folders.AddOrUpdate(
                    f => f.FolderId,
                    new ReminderDb.Folder
                    {
                        FolderId = 1,
                        FolderName = "Default",
                        UserId = 1,
                        FolderIdPerUser = 1
                    }
                    );
                }
                context.SaveChanges();
            }*/

        }
    }
}
