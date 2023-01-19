using System;
using System.Collections.Generic;
using System.Text;

namespace Reenbit.HireMe.Domain.DTOs
{
    public class UserDTO
    {
        public string Email { get; set; }

        public string FullName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsAdmin { get; set; }

        public string TypeUser { get; set; }
    }
}
