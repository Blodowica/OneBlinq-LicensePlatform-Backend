using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.Pagination
{
    public class PaginationUserRequest : PaginationBaseRequest
    {
        //per column
        public string FilterFirstName { get; set; }
        public string FilterLastName { get; set; }
        public string FilterEmail { get; set; }
        public int? FilterLicenseCount  { get; set; }
        public string FilterRole { get; set; }
    }
}
