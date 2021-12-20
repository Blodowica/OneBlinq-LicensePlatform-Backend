using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.Pagination
{
    public class PaginationProductRequest : PaginationBaseRequest
    {
        //per column
        public string FilterProductName { get; set; }
        public string FilterVariantName { get; set; }
        public bool? FilterActive { get; set; }
        public int? FilterLicenseCount { get; set; }
        public int? FilterMaxUses { get; set; }
    }
}
