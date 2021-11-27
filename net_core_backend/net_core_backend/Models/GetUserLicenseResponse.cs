using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class GetUserLicenseResponse
    {
        public int id { get; set; }
        public string ProductName { get; set; }
        public int Activation { get; set; }

        public int MaxUses { get; set; }

        public string Reaccurence { get; set; }

        public  DateTime? ExpirationDate{ get; set; }

        public string Satus { get; set; }

        public string Tier { get; set; }
    }

}
