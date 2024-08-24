using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Models
{
    internal class Contact
    {
        public int ContactID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public bool IsActive { get; set; }
        public List<ContactDetails> ContactDetails { get; set; }

        public Contact()
        {
            ContactDetails = new List<ContactDetails>();
        }
    }
}
