using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Exceptions
{
    public class InactiveUserException : Exception
    {
        public InactiveUserException(string message) : base(message) { }
    }
}
