namespace Mystic_ToDo.Migrations
{
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

            context.SaveChanges();
        }
    }
}
