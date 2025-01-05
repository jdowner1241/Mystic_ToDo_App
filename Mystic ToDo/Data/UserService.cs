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

        public static string RemoveFolderForUser(ReminderContext context, int userId, int folderId)
        {
            var folderPerUser = context.FoldersPerUser.FirstOrDefault(fpu => fpu.UserId == userId && fpu.FolderId == folderId); 
            
            if (folderPerUser == null) 
            { 
                return "Folder not found."; 
            }

            if (folderPerUser.FolderNumberPerUser == 1) 
            { 
                return "The default folder cannot be deleted."; 
            }
                
            var folder = context.Folders.FirstOrDefault(f => f.FolderId == folderId && f.UserId == userId); 
            
            if (folder != null) 
            { 
                context.Folders.Remove(folder); 
                context.FoldersPerUser.Remove(folderPerUser); 
                context.SaveChanges(); 
                return "Folder was deleted successfully."; 
            }
            return "An error occurred. Folder could not be deleted.";
        }

        public static string RemoveUser(ReminderContext context, int userId) 
        { 
            if (userId == 1) 
            { 
                return "The Guest user cannot be deleted."; 
            } 
            
            var user = context.Users.FirstOrDefault(u => u.UserId == userId); 
            
            if (user != null) 
            {

                var foldersPerUser = context.FoldersPerUser.Where(fpu => fpu.UserId == userId).ToList();
                foreach (var fpu in foldersPerUser) 
                { 

                    var folder = context.Folders.FirstOrDefault(f => f.FolderId == fpu.FolderId); 
                    if (folder != null) 
                    { 
                        context.Folders.Remove(folder); 
                    } 
                    context.FoldersPerUser.Remove(fpu); 
                } 

                context.Users.Remove(user); 
                context.SaveChanges(); 
                return "User was deleted successfully."; 
            } 
            
            return "An error occurred. User could not be deleted."; 
        }

    }
}
