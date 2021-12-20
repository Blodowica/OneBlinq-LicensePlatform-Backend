using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.Pagination
{
    public class PaginationLicenseRequest : PaginationBaseRequest
    {
        //per column
        public string FilterLicenseKey { get; set; }
        public string FilterEmail { get; set; }
        public int? FilterActivation { get; set; }
        public string FilterActive { get; set; }
        public string FilterProductName { get; set;}
    }
}
