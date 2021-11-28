using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.Pagination
{
    public class PaginationProductItem
    {

        public int Id { get; set; }
        public string ProductName { get; set; }
        public string VariantName { get; set; }
        public bool Active { get; set; } = true;
        public int LicenseCount { get; set; }
        public int? MaxUses { get; set; }

    }
}
