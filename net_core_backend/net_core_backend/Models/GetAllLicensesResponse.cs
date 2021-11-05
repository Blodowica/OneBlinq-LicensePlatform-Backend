using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class GetAllLicensesResponse
    {
        public int LicenseId { get; set; }
        public string LicenseKey { get; set; }
        public string Email { get; set; }
        public int MaxUses { get; set; }
        public int Activations { get; set; }
        public bool Active { get; set; }
    }
}
