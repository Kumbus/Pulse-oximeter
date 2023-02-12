using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException(string message) : base(message)
        {

        }

        public sealed class InvalidLoginCredentialsException : NotAuthorizedException
        {
            public InvalidLoginCredentialsException() : base("Invalid credentials")
            {
            }
        }
    }
}
