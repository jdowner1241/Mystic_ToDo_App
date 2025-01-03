using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mystic_ToDo.Database.ReminderDb;

namespace Mystic_ToDo.Data
{
    public static class UserService
    {
        public static void CreateInitialUser(ReminderContext context, string userName, string email, string password)
        {
            var newUser = new User
            {
                UserName = userName,
                EmailAddress = email,
                Password = password
            };
            context.Users.Add(newUser);
            context.SaveChanges();

            CreateInitialFolder(context, newUser.UserId);
        }

        public static void CreateInitialFolder(ReminderContext context, int userId)
        {
            var defaultFolder = new Folder
            {
                FolderName = "Default",
                UserId = userId
            };
            context.Folders.Add(defaultFolder);
            context.SaveChanges();

            var folderPerUser = new FolderPerUser
            {
                UserId = userId,
                FolderId = defaultFolder.FolderId,
                FolderNumberPerUser = 1
            };
            context.FoldersPerUser.Add(folderPerUser);
            context.SaveChanges();
        }

        public static void AddNewFolderForUser(ReminderContext context, int userId, string folderName)
        {
            var maxFolderNumber = context.FoldersPerUser
                .Where(fpu => fpu.UserId == userId)
                .Select(fpu => fpu.FolderNumberPerUser)
                .DefaultIfEmpty(0)
                .Max();

            var newFolder = new Folder
            {
                FolderName = folderName,
                UserId = userId
            };
            context.Folders.Add(newFolder);
            context.SaveChanges();

            var newFolderPerUser = new FolderPerUser
            {
                UserId = userId,
                FolderId = newFolder.FolderId,
                FolderNumberPerUser = maxFolderNumber + 1
            };
            context.FoldersPerUser.Add(newFolderPerUser);
            context.SaveChanges();
        }
    }


}
