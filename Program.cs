using ContactApp.Exceptions;
using ContactApp.Models;
using ContactApp.Repositories;

namespace ContactApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //List<User> users = new List<User>();

            //AdminFunctions admin = new AdminFunctions(users);
            //StaffFunctions staff = new StaffFunctions(users);

            // Adding admin
            //admin.CreateUser(new User { UserID = 1, FName = "Admin", LName = "User", IsAdmin = true, IsActive = true });

            //// Admin creats staff
            //admin.CreateUser(new User { UserID = 2, FName = "Staff", LName = "User", IsAdmin = false, IsActive = true });

            //// Staff user creates a contact
            //staff.CreateContact(2, new Contact { ContactID = 1, FName = "John", LName = "Doe", IsActive = false });

            //// Staff user adds contact details
            //staff.CreateContact(2, new Contact
            //{
            //    ContactID = 2,
            //    FName = "Jane",
            //    LName = "Mary",
            //    IsActive = false,
            //    ContactDetails = new List<ContactDetails>
            //{
            //    new ContactDetails { ContactDetailsID = 1, Type = "Phone", Value = "123648744683" },
            //    new ContactDetails { ContactDetailsID = 2, Type = "Email", Value = "jane.mary@examjple.com" }
            //}
            //});

            //// Admin reads all users
            //admin.ReadUsers();

            //// Staff reads all contacts for their user
            //staff.ReadContacts(2);

            //// Updating a contact
            //staff.UpdateContact(2, 1, new Contact { FName = "Johnathanm", LName = "Doe", IsActive = true });

            //// Deleting a contact
            //staff.DeleteContact(2, 2);

            //// Read contacts after updates
            //staff.ReadContacts(2);



            // Initialize users list
            //List<User> users = new List<User>();

            //// Create Active Admin and Inactive Admin users
            //var activeAdminUser = new User { UserID = 1, FName = "Active", LName = "Admin", IsAdmin = true, IsActive = true };
            //var inactiveAdminUser = new User { UserID = 2, FName = "Inactive", LName = "Admin", IsAdmin = true, IsActive = false };
            //var staffUser = new User { UserID = 3, FName = "Staff", LName = "User", IsAdmin = false, IsActive = true };

            //// Add users to the list
            //users.Add(activeAdminUser);
            //users.Add(inactiveAdminUser);
            //users.Add(staffUser);

            //// Instantiate AdminFunctions and StaffFunctions with the current user context
            //AdminFunctions activeAdmin = new AdminFunctions(users, activeAdminUser);
            //AdminFunctions inactiveAdmin = new AdminFunctions(users, inactiveAdminUser);
            //StaffFunctions staff = new StaffFunctions(users, staffUser);

            //// Test Case: Active Admin creating a user
            //Console.WriteLine("Test Case: Active Admin creating a user");
            //activeAdmin.CreateUser(new User { UserID = 4, FName = "New", LName = "User", IsAdmin = false, IsActive = true });
            //activeAdmin.ReadUsers();

            //// Test Case: Inactive Admin trying to create a user (Should Fail)
            //Console.WriteLine("\nTest Case: Inactive Admin trying to create a user (Should Fail)");
            //inactiveAdmin.CreateUser(new User { UserID = 5, FName = "Should", LName = "Fail", IsAdmin = false, IsActive = true });
            //inactiveAdmin.ReadUsers();

            //// Test Case: Staff user trying to create another user (Should Fail)
            //Console.WriteLine("\nTest Case: Staff user trying to create another user (Should Fail)");
            //var staffAsAdmin = new AdminFunctions(users, staffUser);
            //staffAsAdmin.CreateUser(new User { UserID = 6, FName = "Should", LName = "AlsoFail", IsAdmin = false, IsActive = true });
            //staffAsAdmin.ReadUsers();

            //// Test Case: Create Contact for Staff User
            //Console.WriteLine("\nTest Case: Create Contact for Staff User");
            //staff.CreateContact(3, new Contact { ContactID = 1, FName = "John", LName = "Doe", IsActive = true });
            //staff.ReadContacts(3);

            //// Test Case: Inactive Admin trying to delete a user (Should Fail)
            //Console.WriteLine("\nTest Case: Inactive Admin trying to delete a user (Should Fail)");
            //inactiveAdmin.DeleteUser(4);
            //inactiveAdmin.ReadUsers();


            List<User> users = InitializeUsers();
            while (true)
            {
                try
                {
                    Console.Write("Enter UserId: ");
                    int userId = int.Parse(Console.ReadLine());

                    User currentUser = users.FirstOrDefault(u => u.UserID == userId);
                    if (currentUser == null)
                    {
                        throw new UserNotFoundException($"User with ID {userId} not found.");
                    }

                    if (!currentUser.IsActive)
                    {
                        throw new InactiveUserException("The user is inactive and cannot perform any actions.");
                    }

                    if (currentUser.IsAdmin)
                    {
                        AdminFunctions adminFunctions = new AdminFunctions(users, currentUser);
                        AdminMenu(adminFunctions);
                    }
                    else
                    {
                        StaffFunctions staffFunctions = new StaffFunctions(users, currentUser);
                        StaffMenu(staffFunctions, currentUser);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void AdminMenu(AdminFunctions adminFunctions)
        {
            while (true)
            {
                Console.WriteLine("\nAdmin Menu:");
                Console.WriteLine("1. Add new User");
                Console.WriteLine("2. Modify existing user");
                Console.WriteLine("3. Delete user (soft)");
                Console.WriteLine("4. Display all users");
                Console.WriteLine("5. Find user");
                Console.WriteLine("6. Logout");

                int choice = int.Parse(Console.ReadLine());

                try
                {
                    switch (choice)
                    {
                        case 1:
                            User newUser = CreateNewUser();
                            adminFunctions.CreateUser(newUser);
                            break;
                        case 2:
                            Console.Write("Enter User ID to modify: ");
                            int modifyUserId = int.Parse(Console.ReadLine());
                            User modifiedUser = CreateNewUser();
                            adminFunctions.UpdateUser(modifyUserId, modifiedUser);
                            break;
                        case 3:
                            Console.Write("Enter User ID to delete: ");
                            int deleteUserId = int.Parse(Console.ReadLine());
                            adminFunctions.SoftDeleteUser(deleteUserId);
                            break;
                        case 4:
                            adminFunctions.ReadUsers();
                            break;
                        case 5:
                            Console.Write("Enter User ID to find: ");
                            int findUserId = int.Parse(Console.ReadLine());
                            User user = adminFunctions.FindUserById(findUserId);
                            Console.WriteLine($"User Found: ID: {user.UserID}, Name: {user.FName} {user.LName}, IsAdmin: {user.IsAdmin}, IsActive: {user.IsActive}");
                            break;
                        case 6:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void StaffMenu(StaffFunctions staffFunctions, User currentUser)
        {
            while (true)
            {
                Console.WriteLine("\nStaff Menu:");
                Console.WriteLine("1. Work on Contacts");
                Console.WriteLine("   1.1 Add new Contact");
                Console.WriteLine("   1.2 Modify Contact");
                Console.WriteLine("   1.3 Delete Contact (soft)");
                Console.WriteLine("   1.4 Display all Contacts");
                Console.WriteLine("   1.5 Find Contact");
                Console.WriteLine("2. Work on Contact Details");
                Console.WriteLine("3. Logout");

                int choice = int.Parse(Console.ReadLine());

                try
                {
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("\nContacts Menu:");
                            Console.WriteLine("1.1 Add new Contact");
                            Console.WriteLine("1.2 Modify Contact");
                            Console.WriteLine("1.3 Delete Contact (soft)");
                            Console.WriteLine("1.4 Display all Contacts");
                            Console.WriteLine("1.5 Find Contact");
                            int contactChoice = int.Parse(Console.ReadLine());

                            switch (contactChoice)
                            {
                                case 1:
                                    Contact newContact = CreateNewContact();
                                    staffFunctions.CreateContact(currentUser.UserID, newContact);
                                    break;
                                case 2:
                                    Console.Write("Enter Contact ID to modify: ");
                                    int modifyContactId = int.Parse(Console.ReadLine());
                                    Contact modifiedContact = CreateNewContact();
                                    staffFunctions.UpdateContact(currentUser.UserID, modifyContactId, modifiedContact);
                                    break;
                                case 3:
                                    Console.Write("Enter Contact ID to delete: ");
                                    int deleteContactId = int.Parse(Console.ReadLine());
                                    staffFunctions.SoftDeleteContact(currentUser.UserID, deleteContactId);
                                    break;
                                case 4:
                                    staffFunctions.ReadContacts(currentUser.UserID);
                                    break;
                                case 5:
                                    Console.Write("Enter Contact ID to find: ");
                                    int findContactId = int.Parse(Console.ReadLine());
                                    Contact contact = staffFunctions.FindContactById(currentUser.UserID, findContactId);
                                    Console.WriteLine($"Contact Found: ID: {contact.ContactID}, Name: {contact.FName} {contact.LName}, IsActive: {contact.IsActive}");
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice. Please try again.");
                                    break;
                            }
                            break;
                        case 2:
                            Console.WriteLine("Work on Contact Details - functionality to be implemented.");
                            break;
                        case 3:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static User CreateNewUser()
        {
            Console.Write("Enter User ID: ");
            int userId = int.Parse(Console.ReadLine());
            Console.Write("Enter First Name: ");
            string fName = Console.ReadLine();
            Console.Write("Enter Last Name: ");
            string lName = Console.ReadLine();
            Console.Write("Is Admin? (true/false): ");
            bool isAdmin = bool.Parse(Console.ReadLine());
            Console.Write("Is Active? (true/false): ");
            bool isActive = bool.Parse(Console.ReadLine());

            return new User { UserID = userId, FName = fName, LName = lName, IsAdmin = isAdmin, IsActive = isActive };
        }

        static Contact CreateNewContact()
        {
            Console.Write("Enter Contact ID: ");
            int contactId = int.Parse(Console.ReadLine());
            Console.Write("Enter Contact First Name: ");
            string fName = Console.ReadLine();
            Console.Write("Enter Contact Last Name: ");
            string lName = Console.ReadLine();
            Console.Write("Is Active? (true/false): ");
            bool isActive = bool.Parse(Console.ReadLine());

            return new Contact { ContactID = contactId, FName = fName, LName = lName, IsActive = isActive };
        }

        static List<User> InitializeUsers()
        {
            return new List<User>
        {
            new User { UserID = 1, FName = "Admin", LName = "User", IsAdmin = true, IsActive = true },
            new User { UserID = 2, FName = "Staff", LName = "User", IsAdmin = false, IsActive = true }
        };
        }
    }
}

