using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_ToDo.Data
{
    public class ReminderContext : DbContext
    {
        public DbSet <Reminder> Reminders { get; set; }

    }
}
