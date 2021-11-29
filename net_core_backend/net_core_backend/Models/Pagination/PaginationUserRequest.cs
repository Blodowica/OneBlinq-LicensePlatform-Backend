using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.Pagination
{
    public class PaginationUserRequest
    {

        //Filters
        //global
        public string GlobalFilter { get; set; }

        //per column
        public int? FilterId { get; set; }
        public string FilterFirstName { get; set; }
        public string FilterLastName { get; set; }
        public string FilterEmail { get; set; }
        public int? FilterLicenseCount  { get; set; }
        public string FilterRole { get; set; }

        //Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }

    }
}
