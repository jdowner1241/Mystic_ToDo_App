using System.Collections.Generic;
using TaskControl = Mystic_ToDo.View.UserControls.Content.Reminder.ReminderContent.Task;

namespace Mystic_ToDo.View.UserControls
{
    public static class TaskManager
    {
        private static readonly List<TaskControl> allTasks = new List<TaskControl>();

        public static void RegisterTask(TaskControl task)
        {
            if (!allTasks.Contains(task))
            {
                allTasks.Add(task);
            }
        }

        public static void UnregisterTask(TaskControl task)
        {
            if (allTasks.Contains(task))
            {
                allTasks.Remove(task);
            }
        }

        public static IEnumerable<TaskControl> GetAllTasks()
        {
            return allTasks;
        }
    }
}
