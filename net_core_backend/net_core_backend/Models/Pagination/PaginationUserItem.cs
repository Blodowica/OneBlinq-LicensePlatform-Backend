using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.Pagination
{
    public class PaginationUserItem
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int LicenseCount { get; set; }

        public string Role { get; set; }
    }
}
