using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Models
{
    internal class User
    {
        public int UserID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public List<Contact> Contacts { get; set; }

        public User()
        {
            Contacts = new List<Contact>();
        }
    }
}
