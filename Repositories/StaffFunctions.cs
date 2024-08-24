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
    internal class StaffFunctions
    {
        private List<User> users;
        private User currentUser;

        public StaffFunctions(List<User> users, User currentUser)
        {
            this.users = users;
            this.currentUser = currentUser;
        }

        private void ValidateStaff()
        {
            if (!currentUser.IsActive)
            {
                throw new InactiveUserException("Action not allowed. The user is not active.");
            }

            if (currentUser.IsAdmin)
            {
                throw new UnauthorizedAccessException("Action not allowed. Admin cannot perform staff actions.");
            }
        }

        public void CreateContact(int userId, Contact contact)
        {
            ValidateStaff();
            var user = users.FirstOrDefault(u => u.UserID == userId);
            if (user != null)
            {
                user.Contacts.Add(contact);
                Console.WriteLine("Contact created successfully.");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

        public void ReadContacts(int userId)
        {
            ValidateStaff();
            var user = users.FirstOrDefault(u => u.UserID == userId);
            if (user != null)
            {
                foreach (var contact in user.Contacts)
                {
                    Console.WriteLine($"Contact ID: {contact.ContactID}, Name: {contact.FName} {contact.LName}, IsActive: {contact.IsActive}");
                    foreach (var detail in contact.ContactDetails)
                    {
                        Console.WriteLine($"\tDetail ID: {detail.ContactDetailsID}, Type: {detail.Type}, Value: {detail.Value}");
                    }
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

        public void UpdateContact(int userId, int contactId, Contact updatedContact)
        {
            ValidateStaff();
            var user = users.FirstOrDefault(u => u.UserID == userId);
            if (user != null)
            {
                var contact = user.Contacts.FirstOrDefault(c => c.ContactID == contactId);
                if (contact != null)
                {
                    contact.FName = updatedContact.FName;
                    contact.LName = updatedContact.LName;
                    contact.IsActive = updatedContact.IsActive;
                    contact.ContactDetails = updatedContact.ContactDetails;
                    Console.WriteLine("Contact updated successfully.");
                }
                else
                {
                    Console.WriteLine("Contact not found.");
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

        public void SoftDeleteContact(int userId, int contactId)
        {
            ValidateStaff();
            var user = users.FirstOrDefault(u => u.UserID == userId);
            if (user != null)
            {
                var contact = user.Contacts.FirstOrDefault(c => c.ContactID == contactId);
                if (contact != null)
                {
                    contact.IsActive = false;
                    Console.WriteLine("Contact soft deleted (deactivated) successfully.");
                }
                else
                {
                    Console.WriteLine("Contact not found.");
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

        public Contact FindContactById(int userId, int contactId)
        {
            ValidateStaff();
            var user = users.FirstOrDefault(u => u.UserID == userId);
            if (user == null)
            {
                throw new UserNotFoundException($"User with ID {userId} not found.");
            }

            var contact = user.Contacts.FirstOrDefault(c => c.ContactID == contactId);
            if (contact == null)
            {
                throw new UserNotFoundException($"Contact with ID {contactId} not found.");
            }

            return contact;
        }
    }
}
