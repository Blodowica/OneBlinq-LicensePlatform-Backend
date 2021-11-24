using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class PaginationProductRequest
    {

        //Filters
        //global
        public string GlobalFilter { get; set; }

        //per column
        public int? FilterId { get; set; }
        public string FilterProductName { get; set; }
        public string FilterVariantName { get; set; }
        public bool FilterActive { get; set; }
        public int? FilterLicenseCount { get; set; }
        public int? FilterMaxUses { get; set; }


        //Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }

    }
}
