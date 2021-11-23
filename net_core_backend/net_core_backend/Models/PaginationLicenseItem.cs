using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class PaginationLicenseItem
    {
        public int Id { get; set; }
        public string LicenseKey { get; set; }
        public string  Email { get; set; }
        public int Activations { get; set; }
        public int MaxUses { get; set; }
        public bool Active { get; set; }
        public string ProductName { get; set; }

    }
}
