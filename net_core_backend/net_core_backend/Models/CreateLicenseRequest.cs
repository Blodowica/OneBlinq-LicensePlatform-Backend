using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class CreateLicenseRequest
    {
        public string PurchaseLocation { get; set; }
        public string Currency { get; set; }
        public string Recurrence { get; set; }
        public int UserId { get; set; }
        public int Price { get; set; }
        public int ProductId { get; set; }
    }
}
