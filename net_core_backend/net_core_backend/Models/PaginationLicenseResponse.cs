using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class PaginationLicenseResponse
    {
        public List<PaginationLicenseItem> Licenses { get; set; }

        public int MaxPages { get; set; }

    }
}
