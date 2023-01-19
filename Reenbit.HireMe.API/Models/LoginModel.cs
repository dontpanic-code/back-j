using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reenbit.HireMe.API.Models
{
    public class LoginModel
    {
        public string Provider { get; set; }
        public string RedirectUrl { get; set; }
        public string TypeUser { get; set; }
    }
}
