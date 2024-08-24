using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactApp.Exceptions;
using ContactApp.Models;
using UnauthorizedAccessException = ContactApp.Exceptions.UnauthorizedAccessException;

namespace ContactApp.Repositories
{
    internal class AdminFunctions
    {
        private List<User> users;
        private User currentUser;

        public AdminFunctions(List<User> users, User currentUser)
        {
            this.users = users;
            this.currentUser = currentUser;
        }

        private void ValidateAdmin()
        {
            if (!currentUser.IsActive)
            {
                throw new InactiveUserException("Action not allowed. The user is not active.");
            }

            if (!currentUser.IsAdmin)
            {
                throw new UnauthorizedAccessException("Action not allowed. The user is not an admin.");
            }
        }

        public void CreateUser(User user)
        {
            ValidateAdmin();
            users.Add(user);
            Console.WriteLine("User created successfully.");
        }

        public void ReadUsers()
        {
            ValidateAdmin();
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.UserID}, Name: {user.FName} {user.LName}, IsAdmin: {user.IsAdmin}, IsActive: {user.IsActive}");
            }
        }

        public void UpdateUser(int userId, User updatedUser)
        {
            ValidateAdmin();
            var user = users.FirstOrDefault(u => u.UserID == userId);
            if (user != null)
            {
                user.FName = updatedUser.FName;
                user.LName = updatedUser.LName;
                user.IsAdmin = updatedUser.IsAdmin;
                user.IsActive = updatedUser.IsActive;
                Console.WriteLine("User updated successfully.");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

        public void SoftDeleteUser(int userId)
        {
            ValidateAdmin();
            var user = users.FirstOrDefault(u => u.UserID == userId);
            if (user != null)
            {
                user.IsActive = false;
                Console.WriteLine("User soft deleted (deactivated) successfully.");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

        public User FindUserById(int userId)
        {
            ValidateAdmin();
            var user = users.FirstOrDefault(u => u.UserID == userId);
            if (user == null)
            {
                throw new UserNotFoundException($"User with ID {userId} not found.");
            }
            return user;
        }
    }
}
