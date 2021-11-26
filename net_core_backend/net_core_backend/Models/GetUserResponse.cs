using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class GetUserResponse
    {
        public GetUserResponse()
        {
            Licenses = new List<UserLicense>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public List<UserLicense> Licenses { get; set; }

        public class UserLicense
        {
            public string LicenseKey { get; set; }
            public string ProductName { get; set; }
            public int Activations { get; set; }
            public int MaxActivations { get; set; }
        }
    }
}
